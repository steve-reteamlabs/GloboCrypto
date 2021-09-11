using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using GloboCrypto.Models.Authentication;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace GloboCrypto.CoinMonitor
{
    public static class CoinMonitor
    {
        private static AuthToken authToken;

        [Function("CoinMonitor")]
        public async static void Run([TimerTrigger("0 */5 * * * *")] MyInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger("CoinMonitor");
            logger.LogInformation($"C# Timer trigger function started at: {DateTime.Now}");

            var webAPIbase = Environment.GetEnvironmentVariable("GloboCryptoAPI_BASE");
            if (string.IsNullOrEmpty(webAPIbase))
            {
                logger.LogError($"GloboCryptoAPI_BASE is not configured");
                return;
            }

            if (authToken == null || authToken.HasExpired)
            {
                var tokenResponse = await GetAuthToken();
                if (tokenResponse.Result == AuthTokenResponseResult.Success)
                    authToken = tokenResponse.Token;
                else
                    logger.LogError($"Could not get auth token: {tokenResponse.Error}");

                logger.LogInformation($"new token = {authToken.Value}");
            }

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{webAPIbase}/api/Notification/check-and-notify")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Value);

            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                logger.LogInformation($"Check and Notify executed successfully");
            else
                logger.LogError($"Check and Notify failed: [code:{response.StatusCode} => {response.ReasonPhrase}");

            response.EnsureSuccessStatusCode();

            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        private static async Task<AuthTokenResponse> GetAuthToken()
        {
            var id = $"CRYPTO-coin-monitor";
            var webAPIbase = Environment.GetEnvironmentVariable("GloboCryptoAPI_BASE");
            string url = $"{webAPIbase}/api/Auth/authenticate?id={id}";

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            request.Headers.Add("X-API-KEY", "B0E5180C-DDDF-45B3-889A-9E27DCEC2C70");

            using var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var rawToken = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<AuthTokenResponse>(rawToken);
                }
                catch (NotSupportedException)
                {
                    Console.WriteLine("The content type is not supported");
                }
                catch (JsonException)
                {
                    Console.WriteLine("Invalid JSON.");
                }
            }
            return null;
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}

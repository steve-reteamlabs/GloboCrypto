using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace GloboCrypto.CoinMonitor
{
    public static class CoinMonitor
    {
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

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{webAPIbase}/api/Notification/check-and-notify")
            };

            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                logger.LogInformation($"Check and Notify executed successfully");
            else
                logger.LogError($"Check and Notify failed: [code:{response.StatusCode} => {response.ReasonPhrase}");

            response.EnsureSuccessStatusCode();

            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
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

using GloboCrypto.Models.Authentication;
using GloboCrypto.PWA.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GloboCrypto.PWA.Handlers
{
    public class CoinAPIMessageHandler : DelegatingHandler
    {
        private AuthToken AuthToken;
        private readonly IAppStorageService AppStorageService;
        private readonly IAppSettings AppSettings;

        public CoinAPIMessageHandler(IAppStorageService appStorageService, IAppSettings appSettings)
        {
            AppStorageService = appStorageService;
            AppSettings = appSettings;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (AuthToken is null)
                AuthToken = await AppStorageService.GetSavedAuthToken();
            await AuthenticateRequestAsync(request);

            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                AuthToken = null;
                await AuthenticateRequestAsync(request);
                response = await base.SendAsync(request, cancellationToken);
            }
            return response;
        }

        private async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            if (AuthToken?.HasExpired ?? true)
            {
                var authResponse = await GetAuthTokenAsync();
                AuthToken = authResponse.Token;
                await AppStorageService.SaveAuthToken(AuthToken);
            }
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken.Value);
        }

        private async Task<AuthTokenResponse> GetAuthTokenAsync()
        {
            var id2 = await AppStorageService.GetIdAsync();
            var id = $"CRYPTO-{id2}";
            string url = $"{AppSettings.APIHost}/api/Auth/authenticate?id={id}";

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
                    return await response.Content.ReadFromJsonAsync<AuthTokenResponse>();
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
}

using GloboCrypto.Models.Data;
using GloboCrypto.Models.Notifications;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GloboCrypto.PWA.Services
{
    public class CoinAPIService : ICoinAPIService
    {
        private readonly IAppSettings AppSettings;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient HttpClient;

        public CoinAPIService(IAppSettings appSettings, IHttpClientFactory clientFactory)
        {
            AppSettings = appSettings;
            _clientFactory = clientFactory;
            HttpClient = _clientFactory.CreateClient("coinapi");
        }

        public async Task<CoinInfo> GetCoinInfo(string coinId)
        {
            string url = $"{AppSettings.APIHost}/api/Coin/{coinId}";
            return await HttpClient.GetFromJsonAsync<CoinInfo>(url);
        }

        public async Task<IEnumerable<CoinPriceInfo>> GetCoinPriceInfo(string coinIds, string currency = "GBP", string intervals = "1d")
        {
            string url = $"{AppSettings.APIHost}/api/Coin/prices/{coinIds}?currency={currency}";//&intervals={intervals}";
            return await HttpClient.GetFromJsonAsync<IEnumerable<CoinPriceInfo>>(url);
        }

        public async Task SubscribeToNotifications(NotificationSubscription subscription)
        {
            var response = await HttpClient.PutAsJsonAsync($"{AppSettings.APIHost}/api/notifications/subscribe", subscription);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateSubscriptions(string coinIds)
        {
            var response = await HttpClient.GetAsync($"{AppSettings.APIHost}/api/notifications/update-subscription?coinIds={coinIds}");
        }

    }
}

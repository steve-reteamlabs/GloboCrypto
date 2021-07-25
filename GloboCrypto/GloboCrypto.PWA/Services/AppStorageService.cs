using Blazored.LocalStorage;
using GloboCrypto.Models.Data;
using GloboCrypto.PWA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboCrypto.PWA.Services
{
    public class AppStorageService : IAppStorageService
    {
        private readonly IAppSettings AppSettings;
        private readonly ILocalStorageService StorageService;

        public AppStorageService(
            IAppSettings appSettings,
            ILocalStorageService storageService
            )
        {
            AppSettings = appSettings;
            StorageService = storageService;
        }

        private async Task<string> GenerateNewIdAsync()
        {
            var newId = Guid.NewGuid().ToString();
            await StorageService.SetItemAsync(AppSettings.Id, newId);
            return newId;
        }

        public async Task<string> GetIdAsync()
        {
            return await StorageService.GetItemAsync<string>(AppSettings.Id) ?? await GenerateNewIdAsync();
        }


        public async Task<List<CoinInfo>> GetCoinListAsync()
        {
            return (List<CoinInfo>)await StorageService.GetItemAsync<IEnumerable<CoinInfo>>(AppSettings.CoinData);
        }

        public async Task SaveCoinListAsync(IEnumerable<CoinInfo> coinList)
        {
            await StorageService.SetItemAsync(AppSettings.CoinData, coinList);
            await StorageService.SetItemAsync(AppSettings.CacheInvalid, true);
        }

        public async Task<CoinTrackerCache> GetCoinTrackerCacheAsync()
        {
            return await StorageService.GetItemAsync<CoinTrackerCache>(AppSettings.CoinCache);
        }

        public async Task SaveCoinTrackerCacheAsync(CoinTrackerCache coinTrackerCache)
        {
            await StorageService.SetItemAsync(AppSettings.CoinCache, coinTrackerCache);
            await StorageService.SetItemAsync(AppSettings.CacheInvalid, false);
        }

        public async Task<LocalSettings> GetLocalSettingsAsync()
        {
            return await StorageService.GetItemAsync<LocalSettings>(AppSettings.Local);
        }

        public async Task SaveLocalSettingsAsync(LocalSettings settings)
        {
            await StorageService.SetItemAsync(AppSettings.Local, settings);
        }

        public async Task<bool> IsCacheInvalidAsync()
        {
            return await StorageService.GetItemAsync<bool>(AppSettings.CacheInvalid);
        }

    }
}

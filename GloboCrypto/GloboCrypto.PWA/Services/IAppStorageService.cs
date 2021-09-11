using GloboCrypto.Models.Authentication;
using GloboCrypto.Models.Data;
using GloboCrypto.PWA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboCrypto.PWA.Services
{
    public interface IAppStorageService
    {
        Task<string> GetIdAsync();
        Task<List<CoinInfo>> GetCoinListAsync();
        Task SaveCoinListAsync(IEnumerable<CoinInfo> coinList);
        Task<CoinTrackerCache> GetCoinTrackerCacheAsync();
        Task SaveCoinTrackerCacheAsync(CoinTrackerCache coinTrackerCache);
        Task<LocalSettings> GetLocalSettingsAsync();
        Task SaveLocalSettingsAsync(LocalSettings settings);
        Task<bool> IsCacheInvalidAsync();
        Task<AuthToken> GetSavedAuthToken();
        Task SaveAuthToken(AuthToken authToken);
    }
}

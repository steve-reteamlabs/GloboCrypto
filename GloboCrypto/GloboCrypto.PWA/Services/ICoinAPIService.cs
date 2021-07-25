using GloboCrypto.Models.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboCrypto.PWA.Services
{
    public interface ICoinAPIService
    {
        Task<CoinInfo> GetCoinInfo(string coinId);
        Task<IEnumerable<CoinPriceInfo>> GetCoinPriceInfo(string coinIds, string currency = "GBP", string intervals = "1d");

    }
}

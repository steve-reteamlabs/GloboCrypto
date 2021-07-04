using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloboCrypto.Models.Data;

namespace GloboCrypto.WebAPI.Services.Coins
{
    public interface ICoinService
    {
        Task<CoinInfo> GetCoinInfo(string coinId);
        Task<IEnumerable<CoinPriceInfo>> GetCoinPriceInfo(string coinIds, string currency, string intervals);

    }
}

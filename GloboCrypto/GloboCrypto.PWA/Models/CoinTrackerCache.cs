using GloboCrypto.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboCrypto.PWA.Models
{
    public class CoinTrackerCache
    {
        public DateTimeOffset CacheTime { get; set; }
        public IEnumerable<CoinPriceInfo> CoinPrices { get; set; }
    }
}

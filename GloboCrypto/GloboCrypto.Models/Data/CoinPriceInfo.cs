using System;
using System.Collections.Generic;
using System.Text;

namespace GloboCrypto.Models.Data
{
    public class CoinPriceInfoInterval
    {
        public string PriceChange { get; set; }
        public string PriceChangePct { get; set; }
    }

    public class CoinPriceInfo
    {
        public string Id { get; set; }
        public string Price { get; set; }
        public string PriceTimestamp { get; set; }
        public Dictionary<string, CoinPriceInfoInterval> Intervals { get; set; }
    }
}

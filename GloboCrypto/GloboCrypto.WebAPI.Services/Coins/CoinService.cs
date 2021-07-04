using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloboCrypto.Models.Data;
using GloboCrypto.WebAPI.Services.Http;
using GloboCrypto.WebAPI.Services.Models;
using Microsoft.Extensions.Configuration;

namespace GloboCrypto.WebAPI.Services.Coins
{
    public class CoinService : ICoinService
    {
        private readonly IHttpService HttpService;
        private readonly IConfiguration Configuration;

        public CoinService(IConfiguration configuration, IHttpService httpService)
        {
            Configuration = configuration;
            HttpService = httpService;
        }

        private string NomicsAPIKey => Configuration["NomicsAPIKey"];

        public async Task<CoinInfo> GetCoinInfo(string coinId)
        {
            string url = $"https://api.nomics.com/v1/currencies?key={NomicsAPIKey}&ids={coinId}&attributes=id,name,description,logo_url";
            var nomicsCoin = await HttpService.GetAsync<NomicsCoinInfo[]>(url);
            return (nomicsCoin.Length > 0 ? (CoinInfo)nomicsCoin[0] : null);
        }

        public async Task<IEnumerable<CoinPriceInfo>> GetCoinPriceInfo(string coinIds, string currency, string intervals)
        {
            string url = $"https://api.nomics.com/v1/currencies/ticker?key={NomicsAPIKey}&ids={coinIds}&interval={intervals}&convert={currency}";
            var nomicsCoinPrices = await HttpService.GetAsync<NomicsCoinPriceInfo[]>(url);
            return nomicsCoinPrices.Select(x => (CoinPriceInfo)x);
        }

    }
}

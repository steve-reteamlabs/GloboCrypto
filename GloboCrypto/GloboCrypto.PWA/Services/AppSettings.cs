using Microsoft.Extensions.Configuration;

namespace GloboCrypto.PWA.Services
{
    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration Config;

        public AppSettings(IConfiguration config)
        {
            Config = config;
        }

        public string Id => Config["id"];
        public string CoinData => Config["coin-data"];
        public string CoinCache => Config["coin-cache"];
        public string AuthToken => Config["auth-token"];
        public string CacheInvalid => Config["cache-invalid"];
        public string APIHost => Config["api-host"];
        public string Local => Config["app-settings"];
    }
}

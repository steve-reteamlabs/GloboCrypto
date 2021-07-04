using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GloboCrypto.WebAPI.Services.Http
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient HttpClient;

        public HttpService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public Task<T> GetAsync<T>(string url)
        {
            return HttpClient.GetFromJsonAsync<T>(url);
        }

    }
}

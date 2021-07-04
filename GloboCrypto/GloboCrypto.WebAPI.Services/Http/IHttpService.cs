using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GloboCrypto.Models.Data;

namespace GloboCrypto.WebAPI.Services.Http
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string url);
    }
}

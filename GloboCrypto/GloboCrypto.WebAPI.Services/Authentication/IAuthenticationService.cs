using GloboCrypto.Models.Authentication;
using GloboCrypto.WebAPI.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloboCrypto.WebAPI.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthTokenResponse> Authenticate(string id);
        Task<IEnumerable<RegisteredInstance>> GetRegisteredInstances();
    }
}

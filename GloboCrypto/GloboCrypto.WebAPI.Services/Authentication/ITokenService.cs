namespace GloboCrypto.WebAPI.Services.Authentication
{
    public interface ITokenService
    {
        string CreateToken(string identifier);
    }
}

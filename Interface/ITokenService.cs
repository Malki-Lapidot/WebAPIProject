using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebAPIProject.Interface
{
    public interface ITokenService
    {
        SecurityToken GetToken(List<Claim> claims);

        TokenValidationParameters GetTokenValidationParameters();

        string WriteToken(SecurityToken token);
    }
}
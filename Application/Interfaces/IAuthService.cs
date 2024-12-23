using ApplicationProduct.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace ApplicationProduct.Application.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(string id, string username, string role);
        string ComputeSha256Hash(string password);
        JwtSecurityToken ReaderJwt(string tokenJwt);
        Task<JwtSecurityToken> ValidateToken(string tokenJwt);
    }
}

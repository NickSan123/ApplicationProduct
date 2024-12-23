using ApplicationProduct.Domain.Entities;

namespace ApplicationProduct.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Authenticate(string username, string password);
    }
}

using ApplicationProduct.Domain.Entities;

namespace ApplicationProduct.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<IEnumerable<User>> GetUsersByNameAsync(string name);
        Task<User> DisableUserAsync(Guid id);
        public Task<User> VerifyUserCredentialsAsync(string username, string password);
    }
}
using ApplicationProduct.Domain.Entities;

namespace ApplicationProduct.Application.Interfaces
{
    public interface IUserRepository
    {
        public Task AddAsync(User user);
        public Task UpdateAsync(User user);
        public Task<User> GetByIdAsync(Guid id);
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<IEnumerable<User>> FindByNameAsync(string name);
        public Task<User?> GetByUsernameAsync(string username);
        public Task<User?> GetByUsernameAndPassword(string username, string password);
    }
}
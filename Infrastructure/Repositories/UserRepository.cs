using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;
using ApplicationProduct.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApplicationProduct.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users.Include(u => u.Sessions).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.Include(u => u.Sessions).ToListAsync();
        }

        public async Task<IEnumerable<User>> FindByNameAsync(string name)
        {
            return await _context.Users
                .Include(u => u.Sessions)
                .Where(u => EF.Functions.Like(u.Name, $"%{name}%"))
                .ToListAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Sessions)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<User?> GetByUsernameAndPassword(string username, string password)
        {
            return _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == _passwordHasher.HashPassword(password));
        }
    }
}

using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Application.Services;
using ApplicationProduct.Domain.Entities;
using ApplicationProduct.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApplicationProduct.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
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
            var user = await _context.Users.Where(Users => Users.Username == username && Users.Password == password).AsNoTracking()
    .FirstOrDefaultAsync();


            return user;
        }
    }
}

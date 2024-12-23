using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;
using ApplicationProduct.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApplicationProduct.Infrastructure.Repository
{
    public class SessionRepository(AppDbContext dbContext) : ISessionRepository
    {
        private readonly AppDbContext _context = dbContext;

        public async Task<IEnumerable<Session>> GetAllSessionByUserAsync(Guid userId)
        {
            return await _context.Sessions.Where(x=> x.UserId == userId).ToListAsync();
        }

        public async Task<Session?> GetSessionByIdAsync(Guid id)
        {
            return await _context.Sessions.Where(x => x.Id == id).FirstAsync();
        }

        public async Task<Session?> GetSessionByToken(string token)
        {
            return await _context.Sessions.Where(x => x.Token == token).FirstOrDefaultAsync();
        }

        public async Task<Session> LogoutSessionAsync(Session session)
        {
            session.LogoutAt = DateTime.UtcNow;

            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<Session> AddSessionAsync(Session session)
        {
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
            return session;
        }
    }
}

using ApplicationProduct.Application.DTOs;
using ApplicationProduct.Domain.Entities;
using System.Threading.Tasks;

namespace ApplicationProduct.Application.Interfaces
{
    public interface ISessionRepository
    {
        public Task<Session> LogoutSessionAsync(Session session);
        public Task<Session?> GetSessionByIdAsync(Guid id);
        public Task<Session?> GetSessionByToken(string token);
        public Task<IEnumerable<Session>> GetAllSessionByUserAsync(Guid userId);
        public Task<Session> AddSessionAsync(Session session);
    }
}

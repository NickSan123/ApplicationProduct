using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;

namespace ApplicationProduct.Application.Services
{
    public class SessionService (ISessionRepository sessionRepository, IUserService userService): ISessionService
    {
        private readonly ISessionRepository _sessionRepository = sessionRepository;
        private readonly IUserService _userService = userService;
        public async Task<IEnumerable<Session>> GetAllSessionByUserAsync(Guid userId)
        {
            var usuario = await _userService.GetUserByIdAsync(userId);
            return usuario == null ? throw new Exception("Usuario não encontrado!") : await _sessionRepository.GetAllSessionByUserAsync(userId);
        }

        public async Task<Session?> GetSessionByIdAsync(Guid id)
        {
            return await _sessionRepository.GetSessionByIdAsync(id);
        }

        public async Task<Session> LogoutSessionAsync(Guid sessionId)
        {
            var session = await _sessionRepository.GetSessionByIdAsync(sessionId);


            return session == null ? throw new Exception("Sessão não encontrado!") : await _sessionRepository.LogoutSessionAsync(session);
        }
    }
}

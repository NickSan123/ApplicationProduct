using ApplicationProduct.Application.DTOs;
using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;

namespace ApplicationProduct.Application.Services
{
    public class SessionService (ISessionRepository sessionRepository, IUserService userService, IAuthService authService): ISessionService
    {
        private readonly ISessionRepository _sessionRepository = sessionRepository;
        private readonly IUserService _userService = userService;
        private readonly IAuthService _authService = authService;
        public async Task<IEnumerable<Session>> GetAllSessionByUserAsync(Guid userId)
        {
            var usuario = await _userService.GetUserByIdAsync(userId);
            return usuario == null ? throw new Exception("Usuario não encontrado!") : await _sessionRepository.GetAllSessionByUserAsync(userId);
        }

        public async Task<Session?> GetSessionByIdAsync(Guid id)
        {
            return await _sessionRepository.GetSessionByIdAsync(id);
        }

        public async Task<Session?> GetSessionByToken(string token)
        {
            if(token == null)
                throw new ArgumentNullException("token");
           return await _sessionRepository.GetSessionByToken(token);
        }

        public async Task<Session?> LoginSessionAsync(LoginDto dto)
        {
           var user = await _userService.VerifyUserCredentialsAsync(dto.username, dto.password);

            if (user == null)
                return null;

            var token = _authService.GenerateJwtToken(user.Id.ToString(), dto.username, "usuario");
            if (token == null)
                return null;

            var validado = await _authService.ValidateToken(token);
            if (validado == null)
                return null;

            var sessao = new Session
            {
                Id = new Guid(),
                CreatedAt = DateTime.UtcNow,
                LogoutAt = DateTime.UtcNow,
                UserId = user.Id,
                Token = token,
                ExpiresAt = validado.ValidTo
            };
            

            return await _sessionRepository.AddSessionAsync(sessao);
            
        }

        public async Task<Session> LogoutSessionAsync(Guid sessionId)
        {
            var session = await _sessionRepository.GetSessionByIdAsync(sessionId);


            return session == null ? throw new Exception("Sessão não encontrado!") : await _sessionRepository.LogoutSessionAsync(session);
        }
    }
}

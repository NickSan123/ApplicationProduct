
using Application.Interfaces;

namespace WebApi.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;
        private readonly ISessionService _sessionService;

        public JwtMiddleware(RequestDelegate next, ITokenService tokenService, ISessionService sessionService)
        {
            _next = next;
            _tokenService = tokenService;
            _sessionService = sessionService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null && _tokenService.ValidateToken(token, out var userId))
            {
                var session = await _sessionService.CreateSessionAsync(userId, token, DateTime.UtcNow.AddHours(1)); // Exemplo de tempo de expiração
                context.Items["Session"] = session; // Salva a sessão no contexto da requisição
            }

            await _next(context);
        }
    }

}

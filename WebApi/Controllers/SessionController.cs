using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Application.Services;
using ApplicationProduct.WebApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationProduct.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SessionController(ISessionService sessionService, IAuthService authService) : ControllerBase
    {
        private readonly ISessionService _sessionService = sessionService;
        private readonly IAuthService _authService = authService;
        [HttpGet("user/{userID}")]
        public async Task<IActionResult> GetAllSession(Guid userID)
        {
            var dadosToken = Request.Headers["Authorization"];
            if (dadosToken.Count < 1)
            {
                return Unauthorized("Camada 8! Voce deve inserir o token");
            }
           var payload = await _authService.ValidateToken(dadosToken);
            if (payload == null)
            {
                return Unauthorized("Tokem invalido ou espirado");
            }
            try
            {
                return Ok(await _sessionService.GetAllSessionByUserAsync(userID));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

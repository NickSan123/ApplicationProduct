using ApplicationProduct.Application.DTOs;
using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.WebApi.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationProduct.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController(IAuthService authService, ISessionService sessionService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ISessionService _sessionService = sessionService;
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
           var sessao = await  _sessionService.LoginSessionAsync(request);
            if (sessao == null)
            {
                return BadRequest("Login inválido");
            }
            else
            {
                if (string.IsNullOrEmpty(sessao.Token))
                    return BadRequest("Token inválido");
            }
            return Ok(new { sessao.Token });
        }
    }
}

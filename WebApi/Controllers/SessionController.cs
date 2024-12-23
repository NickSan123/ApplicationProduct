using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Application.Services;
using ApplicationProduct.WebApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationProduct.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SessionController(ISessionService sessionService) : ControllerBase
    {
        private readonly ISessionService _sessionService = sessionService;

        [HttpGet("user/{userID}")]
        public async Task<IActionResult> GetAllSession(Guid userID)
        {
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

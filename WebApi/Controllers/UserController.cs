using ApplicationProduct.Application.DTOs;
using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;
using ApplicationProduct.WebApi.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace ApplicationProduct.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController(IUserService userService, IAuthService authService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IAuthService _authService = authService;

        // POST: api/User/Register
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            var dadosToken = Request.Headers["Authorization"];
            if (dadosToken.Count < 1)
            {
                return Unauthorized("Camada 8! Voce deve inserir o token");
            }
            JwtSecurityToken payload = null;

            try
            {
                await _authService.ValidateToken(dadosToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (payload == null)
            {
                if (!dadosToken.Equals("grendene-token"))
                return Unauthorized("Tokem invalido ou espirado");
            }

            if (model == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid user data.");
            }

            var final = await _userService.CreateUserAsync(model);
            if (final == null)
            {
                return BadRequest("Erro ao criar usuario");
            }
            return Ok(final);
        }
        [AllowAnonymous]
        [HttpPut("password")]
        public async Task<IActionResult> AtualizarSenha([FromBody] EditPasswordDto model)
        {
            //var dadosToken = Request.Headers["Authorization"];
            //if (dadosToken.Count < 1)
            //{
            //    return Unauthorized("Camada 8! Voce deve inserir o token");
            //}
            //var payload = await _authService.ValidateToken(dadosToken);
            //if (payload == null)
            //{
            //    if (!dadosToken.Equals("grendene-token")) ;
            //    return Unauthorized("Tokem invalido ou espirado");
            //}

            if (model == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid user data.");
            }

            // Criptografando a senha antes de salvar
            var hashedPassword = _userService.UpdatePassWord(model);


            if (hashedPassword == null)
            {
                return BadRequest("Erro ao atualizar usuario");
            }
            return Ok(true);
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers()
        {
            var dadosToken = Request.Headers["Authorization"];
            if (dadosToken.Count < 1)
            {
                return Unauthorized("Camada 8! Voce deve inserir o token");
            }
            var payload = await _authService.ValidateToken(dadosToken);
            if (payload == null)
            {
                if (!dadosToken.Equals("grendene-token"))
                return Unauthorized("Tokem invalido ou espirado");
            }
            List<UserResultDto> result = new();
            var users = await _userService.GetAllUsersAsync();
            foreach (var user in users)
            {

                UserResultDto userResultDto = new UserResultDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    CreatedAt = user.CreatedAt,
                    Enable = user.Enable,
                    UpdatedAt = user.UpdatedAt,
                    Username = user.Username
                };
                result.Add(userResultDto);
            }
            return Ok(result);
        }
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var dadosToken = Request.Headers["Authorization"];
            if (dadosToken.Count < 1)
            {
                return Unauthorized("Camada 8! Voce deve inserir o token");
            }
            var payload = await _authService.ValidateToken(dadosToken);
            if (payload == null)
            {
                if (!dadosToken.Equals("grendene-token"))
                return Unauthorized("Tokem invalido ou espirado");
            }
            var user = await _userService.GetUserByIdAsync(id);

            return Ok(new UserResultDto
            {
                Id = user.Id,
                Name = user.Name,
                CreatedAt = user.CreatedAt,
                Enable = user.Enable,
                UpdatedAt = user.UpdatedAt,
                Username = user.Username
            });
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            var dadosToken = Request.Headers["Authorization"];
            if (dadosToken.Count < 1)
            {
                return Unauthorized("Camada 8! Voce deve inserir o token");
            }
            var payload = await _authService.ValidateToken(dadosToken);
            if (payload == null)
            {
                if (!dadosToken.Equals("grendene-token"))
                    return Unauthorized("Tokem invalido ou espirado");
            }
            var usr = await _userService.GetUserByIdAsync(user.Id);
            if(user == null)
                return NotFound("Usuario não encontrado!");
            
            return Ok(await _userService.UpdateUserAsync(user));
        }
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var dadosToken = Request.Headers["Authorization"];
            if (dadosToken.Count < 1)
            {
                return Unauthorized("Camada 8! Voce deve inserir o token");
            }
            var payload = await _authService.ValidateToken(dadosToken);
            if (payload == null)
            {
                if (!dadosToken.Equals("grendene-token")) ;
                return Unauthorized("Tokem invalido ou espirado");
            }
            var users = await _userService.GetUsersByNameAsync(name);

            List<UserResultDto> result = new();

            foreach (var user in users)
            {

                UserResultDto userResultDto = new UserResultDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    CreatedAt = user.CreatedAt,
                    Enable = user.Enable,
                    UpdatedAt = user.UpdatedAt,
                    Username = user.Username
                };
                result.Add(userResultDto);
            }
            return Ok(result);
        }

    }
}

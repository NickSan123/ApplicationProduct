using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;
using ApplicationProduct.WebApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationProduct.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserService _userService; 
        public UserController(IPasswordHasher passwordHasher, IUserService userService)
        {
            _passwordHasher = passwordHasher;
            _userService = userService;
        }

        // POST: api/User/Register
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid user data.");
            }

            // Criptografando a senha antes de salvar
            var hashedPassword = _passwordHasher.HashPassword(model.Password);

            model.Password = hashedPassword;
           var final = await _userService.CreateUserAsync(model);
            if (final == null)
            {
                return BadRequest("Erro ao criar usuario");
            }
            return Ok(final);
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers()
        {
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
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var users = await _userService.GetUsersByNameAsync(name);

            List<UserResultDto> result = new ();

            foreach (var user in users) {

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

        //// POST: api/User/Login
        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        //{
        //    if (model == null || !ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid login data.");
        //    }

        //    var user = await _userService.GetUsersByNameAsync(model.Username); // Recupera o usuário pelo nome

        //    if (user == null)
        //    {
        //        return Unauthorized("Invalid credentials.");
        //    }

        //    // Verificando se a senha informada bate com o hash armazenado
        //    var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

        //    if (passwordVerificationResult == PasswordVerificationResult.Failed)
        //    {
        //        return Unauthorized("Invalid credentials.");
        //    }

        //    // Se a senha for válida, você pode gerar o JWT ou outro tipo de token para autenticação
        //    var token = GenerateJwtToken(user); // Método fictício para gerar um JWT

        //    return Ok(new { Token = token });
        //}

    }
}

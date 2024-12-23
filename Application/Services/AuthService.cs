using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;


namespace ApplicationProduct.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _key;
        private readonly string _issuer;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _key = configuration["Jwt:Key"] ?? "chave-secreta-default123";
            _issuer = configuration["Jwt:Issuer"] ?? "localhost";
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAndPassword(username, _passwordHasher.HashPassword(password));
            if (user == null || !user.Enable)
            {
                return null; // Usuário inválido
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, "User") // Adicione roles conforme necessário
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

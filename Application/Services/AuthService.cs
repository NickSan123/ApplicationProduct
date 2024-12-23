using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;


namespace ApplicationProduct.Application.Services
{
    public class AuthService(IConfiguration configuration, IUserRepository userRepository, ISessionRepository sessionRepository) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISessionRepository _sessionRepository = sessionRepository;

        public string ComputeSha256Hash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public string GenerateJwtToken(string id, string username, string role)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var ExpiryHours = Convert.ToInt32(_configuration["Jwt:ExpiryHours"]);
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("id", id.ToString()),
                new Claim("username", username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.Now.AddMinutes(ExpiryHours),
                signingCredentials: credentials,
                claims: claims);

            var tokenHandler = new JwtSecurityTokenHandler();

            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }

        public JwtSecurityToken ReaderJwt(string tokenJwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenJwt);

            return token;
        }

        public async Task<JwtSecurityToken?> ValidateToken(string tokenJwt)
        {
            if (tokenJwt == null)
                throw new Exception("Token Inválido");

            var token = ReaderJwt(tokenJwt.Replace("Bearer", "").Trim());

            if (token.Payload == null)
                return null;

            if (!token.Payload.ContainsKey("id"))
                return null;

            if (!token.Payload.ContainsKey("username"))
                return null;

            if (token.ValidTo < DateTime.Now)
                return null;

            token.Payload.TryGetValue("id", out var userId).ToString();

            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId.ToString()));

            if (user == null)
                return null;

            if (!user.Enable)
                return null;

            return token;

            //if (string.IsNullOrWhiteSpace(tokenJwt))
            //    throw new ArgumentException("Token inválido.");

            //// Remove o prefixo "Bearer" se presente
            //var token = tokenJwt.Replace("Bearer", "", StringComparison.OrdinalIgnoreCase).Trim();

            //var handler = new JwtSecurityTokenHandler();

            //if (!handler.CanReadToken(token))
            //    throw new SecurityTokenException("Token JWT não pode ser lido.");

            //// Parâmetros de validação do token
            //var validationParameters = new TokenValidationParameters
            //{
            //    ValidateIssuer = false,
            //    ValidateAudience = false,
            //    ValidateLifetime = true,
            //    ValidateIssuerSigningKey = false,
            //};

            //try
            //{
            //    var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

            //    // Verifica se o token é um JwtSecurityToken
            //    if (validatedToken is not JwtSecurityToken jwtToken)
            //        throw new SecurityTokenException("Token JWT inválido.");

            //    // Obtém as claims do token
            //    var idClaim = principal.FindFirst("id")?.Value;
            //    var usernameClaim = principal.FindFirst("username")?.Value;

            //    if (string.IsNullOrEmpty(idClaim))
            //        throw new SecurityTokenException("Token inválido: falta o 'id'.");

            //    if (string.IsNullOrEmpty(usernameClaim))
            //        throw new SecurityTokenException("Token inválido: falta o 'username'.");

            //    // Verifica se o token está expirado
            //    if (jwtToken.ValidTo < DateTime.UtcNow)
            //        throw new SecurityTokenExpiredException("Token expirado.");

            //    // Converte o 'id' para Guid
            //    if (!Guid.TryParse(idClaim, out var userId))
            //        throw new SecurityTokenException("ID do usuário no token é inválido.");

            //    // Obtém o usuário do repositório
            //    var user = await _userRepository.GetByIdAsync(userId);

            //    if (user == null)
            //        throw new SecurityTokenException("Usuário não encontrado.");

            //    if (!user.Enable)
            //        throw new SecurityTokenException("Usuário inativo.");

            //    return jwtToken;
            //}
            //catch (SecurityTokenException ex)
            //{
            //    // Log de segurança ou tratamento adicional
            //    throw new SecurityTokenException($"Erro na validação do token: {ex.Message}", ex);
            //}
            //catch (Exception ex)
            //{
            //    // Tratamento genérico de exceções
            //    throw new Exception($"Erro inesperado na validação do token: {ex.Message}", ex);
            //}
        }
    }
}

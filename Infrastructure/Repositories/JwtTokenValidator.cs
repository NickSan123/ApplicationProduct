using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProduct.Infrastructure.Repositories
{
    public class JwtTokenValidator : IJwtTokenValidator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenPayload? ValidateToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero // Evita tolerância para expirado
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    // Extraia as informações do payload
                    return new TokenPayload
                    {
                        UserId = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value,
                        UserName = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value,
                        Role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                        Expiration = jwtToken.ValidTo
                    };
                }
            }
            catch (Exception)
            {
                // Token inválido ou expirado
                return null;
            }

            return null;
        }
    }

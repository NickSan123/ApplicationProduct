using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TokenService
    {
        private readonly string _secretKey;
        private readonly SigningCredentials _signingCredentials;

        public TokenService(IConfiguration configuration)
        {
            _secretKey = configuration.GetValue<string>("Jwt:SecretKey");
            var key = Encoding.ASCII.GetBytes(_secretKey);
            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        }
    }
}

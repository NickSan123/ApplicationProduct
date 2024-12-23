using ApplicationProduct.Application.DTOs;
using ApplicationProduct.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProduct.Application.Services
{
    public class AuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthenticationResult> Authenticate(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAndPassword(username, password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = _jwtTokenGenerator.GenerateToken(user.Id + "", user.Username, "usuario");

            return new AuthenticationResult(user.Id + "", user.Username, token);
        }
    }

}

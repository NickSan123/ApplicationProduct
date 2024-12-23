using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProduct.Application.DTOs
{
    public class AuthenticationResult
    {
        public string UserId { get; }
        public string UserName { get; }
        public string Token { get; }

        public AuthenticationResult(string userId, string userName, string token)
        {
            UserId = userId;
            UserName = userName;
            Token = token;
        }
    }

}

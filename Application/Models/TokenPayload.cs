using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationProduct.Application.Models
{
    public class TokenPayload
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public DateTime Expiration { get; set; }
    }

}

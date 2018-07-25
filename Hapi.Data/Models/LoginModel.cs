using Hapi.Data.Abstracts;
using Hapi.Data.Enumerations;

namespace Hapi.Data.Models
{
    public class LoginModel : ILogin
    {
        public TokenAuthTypes AuthTypes { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DomainName { get; set; }
    }
}

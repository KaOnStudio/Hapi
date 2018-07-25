using Hapi.Data.Abstracts;

namespace Hapi.Data.Models
{
    public class AppLoginModel:IAppLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

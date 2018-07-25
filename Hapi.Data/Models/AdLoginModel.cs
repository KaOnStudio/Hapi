using Hapi.Data.Abstracts;

namespace Hapi.Data.Models
{
    public class AdLoginModel:IAdLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DomainName { get; set; }
    }
}

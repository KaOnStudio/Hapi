using Hapi.Data.Abstracts;

namespace Hapi.Data.Models
{
    public class TokenResponse:ITokenResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
    }
}

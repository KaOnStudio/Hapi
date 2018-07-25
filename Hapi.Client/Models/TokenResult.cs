using System.Collections.Generic;
using Hapi.Client.Abstracts;
using RestSharp;

namespace Hapi.Client.Models
{
    internal class TokenResult:ITokenResult
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public IList<RestResponseCookie> Cookies { get; set; }
    }
}

using System;
using System.Linq;
using Hapi.Client.Abstracts;
using RestSharp;

namespace Hapi.Client.Authenticators
{
    internal class BearerTokenAuthenticator:IAuthenticator
    {
        private ITokenResult _tokenResult;

        public BearerTokenAuthenticator(ITokenResult tokenResult)
        {
            _tokenResult = tokenResult;
        }

        public void SetResult(ITokenResult tokenResult)
        {
            _tokenResult = tokenResult;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (!request.Parameters.Any(p => p.Name.Equals("content-type", StringComparison.OrdinalIgnoreCase)))
                request.AddParameter("content-type", "application/json", ParameterType.HttpHeader);

            if (!request.Parameters.Any(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase)))
                request.AddParameter("Authorization", $"{_tokenResult.TokenType} {_tokenResult.AccessToken}", ParameterType.HttpHeader);           
        }
    }
}

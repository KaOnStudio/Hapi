using System.Threading;
using System.Threading.Tasks;
using Hapi.Data.Abstracts;
using RestSharp;

namespace Hapi.Client.Abstracts
{
    public interface ITokenHelper
    {
        void SetAuthenticationModel(IAppLogin appLoginModel);
        void SetAuthenticationModel(IAdLogin adLoginModel);
        ITokenResult GetToken(IRestClient client);
        Task<ITokenResult> GetTokenAsync(IRestClient client, CancellationToken cancellationToken);
    }
}
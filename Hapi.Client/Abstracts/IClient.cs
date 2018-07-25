using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace Hapi.Client.Abstracts
{
    public interface IClient
    {
        T Request<T>(string url, Method method, bool useToken, bool useRefreshToken = true, List<Parameter> parameters = null) where T : new();
        Task<T> RequestAsync<T>(string url, Method method, bool useToken, CancellationToken cancellationToken, bool useRefreshToken = true, List<Parameter> parameters = null) 
            where T : new();
        bool ClearToken();
        string GetToken();
    }
}
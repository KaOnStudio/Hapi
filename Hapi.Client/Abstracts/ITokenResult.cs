using Hapi.Data.Abstracts;

namespace Hapi.Client.Abstracts
{
    public interface ITokenResult:ITokenResponse
    {
        bool IsSuccessful { get; set; }
        string Message { get; set; }
    }
}
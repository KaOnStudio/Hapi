namespace Hapi.Client.Abstracts
{
    public interface IAuthenticator:RestSharp.Authenticators.IAuthenticator
    {
        void SetResult(ITokenResult result);
    }
}
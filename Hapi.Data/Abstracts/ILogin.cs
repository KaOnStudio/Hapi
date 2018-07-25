using Hapi.Data.Enumerations;

namespace Hapi.Data.Abstracts
{
    public interface ILogin:IAdLogin, IAppLogin
    {
        TokenAuthTypes AuthTypes { get; set; }
    }
}
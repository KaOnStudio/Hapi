using Hapi.Data.Abstracts;

namespace Hapi.Client.Abstracts
{
    public interface ISettings
    {
        string ApiPath { get; set; }
        IAppLogin AppLoginModel { get; set; }
        IAdLogin AdLoginModel { get; set; }
    }
}
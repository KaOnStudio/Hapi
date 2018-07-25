using Hapi.Client.Abstracts;
using Hapi.Data.Abstracts;

namespace Hapi.Client
{
    public class Settings:ISettings
    {
        public string ApiPath { get; set; } = "http://localhost:19714/";
        public IAppLogin AppLoginModel { get; set; }
        public IAdLogin AdLoginModel { get; set; }
    }
}

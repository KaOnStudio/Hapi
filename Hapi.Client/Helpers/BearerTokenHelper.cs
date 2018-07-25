using System;
using System.Threading;
using System.Threading.Tasks;
using Hapi.Client.Abstracts;
using Hapi.Common.Abstracts;
using Hapi.Data;
using Hapi.Data.Abstracts;
using Hapi.Data.Enumerations;
using Hapi.Data.Models;
using RestSharp;
using Microsoft.Extensions.DependencyInjection;
namespace Hapi.Client.Helpers
{
    internal class BearerTokenHelper:ITokenHelper
    {
        #region Variables

        private readonly IDependencyContext _context;
        private TokenAuthTypes _authType;
        private IAppLogin _appLogin;
        private IAdLogin _adLogin;

        #endregion

        #region Constructure

        public BearerTokenHelper(IDependencyContext context)
        {
            _context = context;
        }

        #endregion
        
        #region Public Methods

        public void SetAuthenticationModel(IAppLogin appLoginModel)
        {
            _appLogin = appLoginModel;
            _authType = TokenAuthTypes.AppLogin;
        }
        public void SetAuthenticationModel(IAdLogin adLoginModel)
        {
            _adLogin = adLoginModel;
            _authType = TokenAuthTypes.AdLogin;
        }
        public ITokenResult GetToken(IRestClient client)
        {
            try
            {
                var request = CreateTokenRequest();
                var response = client.Execute<TokenResponse>(request);
                return ConvertToTokenResult(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Sunucudan Token alma işleminde Hata!", ex);
            }
        }
        public async Task<ITokenResult> GetTokenAsync(IRestClient client,CancellationToken cancellationToken)
        {
            try
            {
                var request = CreateTokenRequest();
                var response = await client.ExecuteTaskAsync<TokenResponse>(request,cancellationToken);
                return ConvertToTokenResult(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Sunucudan Token alma işleminde Hata!", ex);
            }
        }

        #endregion

        #region Private Methods

        private IRestRequest CreateTokenRequest()
        {
            try
            {
                var request = new RestRequest(Constants.TokenPath, Method.POST);

                //Header Bilgisi ekleniyor
                request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
                request.AddHeader("accept-charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
                request.AddHeader("accept-language", "tr-Tr, tr;q=0.8;en-US;q=0.6,en;q=0.4;");
                request.AddHeader("accept-encoding", "gzip,deflate");
                request.AddHeader("content-type", "application/json");

                ILogin loginModel = _context.ServiceProvider.GetService<ILogin>();
                
                //Token isteği için Body kısmı ekleniyor
                switch (_authType)
                {
                    case TokenAuthTypes.AppLogin:
                        loginModel.AuthTypes = TokenAuthTypes.AppLogin;
                        loginModel.Username = _appLogin.Username;
                        loginModel.Password = _appLogin.Password;
                        break;
                    case TokenAuthTypes.AdLogin:
                        loginModel.AuthTypes = TokenAuthTypes.AppLogin;
                        loginModel.Username = _adLogin.Username;
                        loginModel.Password = _adLogin.Password;
                        loginModel.DomainName = _adLogin.DomainName;
                        break;
                    default:
                        throw new Exception($"Geçersiz {_authType.ToString()} değeri");
                }
                var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(loginModel);
                request.AddParameter("application/json", parameters, ParameterType.RequestBody);
                return request;
            }
            catch (Exception e)
            {
                throw new Exception("Token isteği oluşturulurken beklenmedik bir hata oluştu",e);
            }
        }
        private ITokenResult ConvertToTokenResult(IRestResponse<TokenResponse> response)
        {
            try
            {
                //Token isteği sonucunda 200 yanıtı alınmadı ise hata fırlatılıyor
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw response.ErrorException;

                var result = _context.ServiceProvider.GetService<ITokenResult>();
                result.AccessToken = response.Data?.AccessToken;
                result.TokenType = response.Data?.TokenType;
                result.IsSuccessful = response.IsSuccessful;
                result.Message = response.ErrorMessage;
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Token isteği dönüşümü sırasında beklenmeyen bir sorun oluştu",e);
            }
        }

        #endregion
        
    }
}

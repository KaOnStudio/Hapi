using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hapi.Common.Abstracts;
using Hapi.Data.Abstracts;
using Hapi.Data.Models;
using RestSharp;
using Microsoft.Extensions.DependencyInjection;
namespace Hapi.Client.Abstracts
{
    public abstract class ClientBase:IClient
    {
        private ITokenResult _tokenResult;
        private readonly ISettings _settings;

        protected IDependencyContext Context { get; }
        protected ClientBase(IDependencyContext context,ISettings settings)
        {
            Context = context;
            _settings = settings;
        }

        #region Public Methods

         public T Request<T>(string url, Method method, bool useToken, bool useRefreshToken = true, List<Parameter> parameters = null) where T : new()
        {
            try
            {
                if (!useToken)
                    return RequestCore<T>(url, method, false, useRefreshToken, parameters);

                if (_settings.AppLoginModel != null)
                    return RequestCore<T>(url, method, true, useRefreshToken, parameters, appLoginModel: _settings.AppLoginModel);

                if (_settings.AdLoginModel != null)
                    return RequestCore<T>(url, method, true, useRefreshToken, parameters, adLoginModel: _settings.AdLoginModel);

                throw new Exception("Token için gerekli olan tanımlamalar eksik");
            }
            catch (Exception e)
            {
                throw  new Exception("Istek işlenirken hata oluştu",e);
            }
        }

        public async Task<T> RequestAsync<T>(string url, Method method, bool useToken, CancellationToken cancellationToken, 
            bool useRefreshToken = true, List<Parameter> parameters = null) where T : new()
        {
            try
            {
                if (!useToken)
                    return await RequestCoreAsync<T>(url, method, false, useRefreshToken, cancellationToken, parameters);

                if (_settings.AppLoginModel != null)
                    return await RequestCoreAsync<T>(url, method, true, useRefreshToken, cancellationToken, parameters, appLoginModel: _settings.AppLoginModel);

                if (_settings.AdLoginModel != null)
                    return await RequestCoreAsync<T>(url, method, true, useRefreshToken, cancellationToken, parameters, adLoginModel: _settings.AdLoginModel);

                throw new Exception("Token için gerekli olan tanımlamalar eksik");
            }
            catch (Exception e)
            {
                throw  new Exception("Istek işlenirken hata oluştu",e);
            }
        }

        public bool ClearToken()
        {
            _tokenResult = null;
            return true;
        }

        public string GetToken()
        {
            return $"{_tokenResult?.TokenType} {_tokenResult?.AccessToken}";
        }

        #endregion       

        #region Private Methods
        
        private T RequestCore<T>(string url, Method method, bool useAuth, bool useRefreshToken,
            List<Parameter> parameters = null, IAppLogin appLoginModel = null, IAdLogin adLoginModel = null, bool isRetry = false) where T : new()
        {
            try
            {
                IRestClient client = new RestClient(_settings.ApiPath);
                IAuthenticator authenticator = GetAuthenticator(client, useAuth, appLoginModel, adLoginModel);

                if (authenticator != null)
                    client.Authenticator = authenticator;

                IRestRequest req = CreateRequest(url, method, parameters);
                IRestResponse<ResultModel<T>> resp = client.Execute<ResultModel<T>>(req);

                switch (resp.StatusCode)
                {
                    case System.Net.HttpStatusCode.Unauthorized when !useAuth:
                        throw resp.ErrorException;
                    case System.Net.HttpStatusCode.Unauthorized when isRetry || !useRefreshToken:
                        throw resp.ErrorException;
                    case System.Net.HttpStatusCode.Unauthorized:
                        _tokenResult = null;
                        return  RequestCore<T>(url, method, true, true, parameters, appLoginModel, adLoginModel, true);
                    case System.Net.HttpStatusCode.OK:
                        return resp.Data.Data;
                    default:
                        throw resp.ErrorException;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Istek Sırasında beklenmeyen bir sorun oluştu", ex);
            }
        }
        private async Task<T> RequestCoreAsync<T>(string url, Method method, bool useAuth, bool useRefreshToken, CancellationToken cancellationToken,
            List<Parameter> parameters = null, IAppLogin appLoginModel = null, IAdLogin adLoginModel = null, bool isRetry = false) where T : new()
        {
            try
            {
                IRestClient client = new RestClient(_settings.ApiPath);
                IAuthenticator authenticator = await GetAuthenticatorAsync(client, useAuth, cancellationToken, appLoginModel, adLoginModel);

                if (authenticator != null)
                    client.Authenticator = authenticator;

                IRestRequest req = CreateRequest(url, method, parameters);
                IRestResponse<ResultModel<T>> resp = await client.ExecuteTaskAsync<ResultModel<T>>(req, cancellationToken);

                switch (resp.StatusCode)
                {
                    case System.Net.HttpStatusCode.Unauthorized when !useAuth:
                        throw resp.ErrorException;
                    case System.Net.HttpStatusCode.Unauthorized when isRetry || !useRefreshToken:
                        throw resp.ErrorException;
                    case System.Net.HttpStatusCode.Unauthorized:
                        _tokenResult = null;
                        return await RequestCoreAsync<T>(url, method, true, true, cancellationToken, parameters, appLoginModel, adLoginModel, true);
                    case System.Net.HttpStatusCode.OK:
                        return resp.Data.Data;
                    default:
                        throw resp.ErrorException;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Istek Sırasında beklenmeyen bir sorun oluştu", ex);
            }
        }
        private IAuthenticator GetAuthenticator(IRestClient client, bool useAuth,IAppLogin appLoginModel = null, IAdLogin adLoginModel = null)
        {
            try
            {
                if (!useAuth)
                    return null;

                var authenticator = Context.ServiceProvider.GetService<IAuthenticator>();
                var helper = Context.ServiceProvider.GetService<ITokenHelper>();

                if (_tokenResult != null)
                {
                    authenticator.SetResult(_tokenResult);
                    return authenticator;
                }

                if (appLoginModel != null)
                    helper.SetAuthenticationModel(appLoginModel);
                else if (adLoginModel != null)
                    helper.SetAuthenticationModel(adLoginModel);
                else
                    throw new Exception("Token isteği için uygum model bulunamadı");

                _tokenResult = helper.GetToken(client);

                if (!_tokenResult?.IsSuccessful ?? true)
                    throw new Exception("Token Getirilemedi");

                authenticator.SetResult(_tokenResult);
                return authenticator;
            }
            catch (Exception ex)
            {
                throw new Exception("Token bilgisi alınırken beklenmeyen bir sorun oluştu!",ex);
            }
        }
        private async Task<IAuthenticator> GetAuthenticatorAsync(IRestClient client, bool useAuth, CancellationToken cancellationToken,
            IAppLogin appLoginModel = null, IAdLogin adLoginModel = null)
        {
            try
            {
                if (!useAuth)
                    return null;

                var authenticator = Context.ServiceProvider.GetService<IAuthenticator>();
                var helper = Context.ServiceProvider.GetService<ITokenHelper>();

                if (_tokenResult != null)
                {
                    authenticator.SetResult(_tokenResult);
                    return authenticator;
                }

                if (appLoginModel != null)
                    helper.SetAuthenticationModel(appLoginModel);
                else if (adLoginModel != null)
                    helper.SetAuthenticationModel(adLoginModel);
                else
                    throw new Exception("Token isteği için uygum model bulunamadı");

                _tokenResult = await helper.GetTokenAsync(client, cancellationToken);

                if (!_tokenResult?.IsSuccessful ?? true)
                    throw new Exception("Token Getirilemedi");

                authenticator.SetResult(_tokenResult);
                return authenticator;
            }
            catch (Exception ex)
            {
                throw new Exception("Token bilgisi alınırken beklenmeyen bir sorun oluştu!",ex);
            }
        }
        private IRestRequest CreateRequest(string url, Method method, List<Parameter> parameters)
        {
            try
            {
                IRestRequest req = new RestRequest(url, method);

                if (parameters == null)
                    return req;

                //İstek Header kısmı oluşturuluyor
                req.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
                req.AddHeader("accept-charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
                req.AddHeader("accept-language", "tr-Tr, tr;q=0.8;en-US;q=0.6,en;q=0.4;");
                req.AddHeader("accept-encoding", "gzip,deflate");
                req.AddHeader("content-type", "application/x-www-form-urlencoded");

                //İstek gövdesi oluşturuluyor
                foreach (Parameter parameter in parameters)
                {
                    switch (parameter.Type)
                    {
                        case ParameterType.UrlSegment:
                            req.AddUrlSegment(parameter.Name, parameter.Value.ToString());
                            break;
                        case ParameterType.RequestBody:
                            string prmtr = string.Join("&", parameters.Select(x => $"{x.Name}={x.Value}"));
                            req.AddParameter("application/x-www-form-urlencoded", prmtr, ParameterType.RequestBody);
                            break;
                        case ParameterType.HttpHeader:
                            req.AddHeader(parameter.Name, parameter.Value.ToString());
                            break;
                        case ParameterType.QueryString:
                            req.AddQueryParameter(parameter.Name, parameter.Value.ToString());
                            break;
                        default:
                            req.AddParameter(parameter.Name, parameter.Value);
                            break;
                    }
                }

                return req;
            }
            catch (Exception ex)
            {
                throw new Exception("İstek oluşturma işleminde beklenmeyen bir sorun oluştu!",ex);
            }
        }

        #endregion
    }
}

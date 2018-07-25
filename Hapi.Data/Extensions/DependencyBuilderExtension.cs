using Hapi.Common.DependencyInjections;
using Hapi.Data.Abstracts;
using Hapi.Data.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hapi.Data.Extensions
{
    public static class DependencyBuilderExtension
    {
        public static DependencyBuilder AddHapiData(this DependencyBuilder builder)
        {
            builder.Services.TryAddTransient(typeof(IResultModel<>), typeof(ResultModel<>));
            builder.Services.TryAddTransient<ITokenResponse, TokenResponse>();
            builder.Services.TryAddTransient<IAppLogin, AppLoginModel>();
            builder.Services.TryAddTransient<IAdLogin, AdLoginModel>();
            builder.Services.TryAddTransient<ILogin, LoginModel>();
            builder.Context.BuildServiceProvider();
            return builder;
        }
    }
}

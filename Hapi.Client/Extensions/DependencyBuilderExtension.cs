using Hapi.Client.Abstracts;
using Hapi.Client.Authenticators;
using Hapi.Client.Helpers;
using Hapi.Client.Models;
using Hapi.Common.DependencyInjections;
using Hapi.Data.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hapi.Client.Extensions
{
    public static class DependencyBuilderExtension
    {
        public static DependencyBuilder AddHapiClient(this DependencyBuilder builder)
        {
            builder.AddHapiData();
            builder.Services.TryAddTransient<ITokenResult, TokenResult>();
            builder.Services.TryAddTransient<ITokenHelper, BearerTokenHelper>();
            builder.Services.TryAddTransient<IAuthenticator, BearerTokenAuthenticator>();
            builder.Services.TryAddSingleton<ISettings, Settings>();
            builder.Context.BuildServiceProvider();
            return builder;
        }
    }
}

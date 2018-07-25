using Hapi.Client.Extensions;
using Hapi.Client.Pokemon.Abstracts;
using Hapi.Common.DependencyInjections;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hapi.Client.Pokemon.Extensions
{
    public static class DependencyBuilderExtension
    {
        public static DependencyBuilder AddPokemonClient(this DependencyBuilder builder)
        {
            builder.AddHapiClient();
            builder.Services.TryAddTransient<IPokemonClient, PokemonClient>();
            builder.Context.BuildServiceProvider();
            return builder;
        }
    }
}

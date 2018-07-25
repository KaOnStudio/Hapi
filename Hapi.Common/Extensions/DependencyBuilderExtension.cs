using System;
using Hapi.Common.Abstracts;
using Hapi.Common.DependencyInjections;
using Microsoft.Extensions.DependencyInjection;

namespace Hapi.Common.Extensions
{
    public static class DependencyBuilderExtension
    {
        public static DependencyBuilder AddDependencyInfrastructure(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            DependencyBuilder builder = new DependencyBuilder(services);

            services.AddSingleton(typeof(IDependencyContext), s => builder.Context);

            return builder;
        }
    }
}

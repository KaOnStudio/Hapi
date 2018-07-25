using Hapi.Client.Book.Abstracts;
using Hapi.Client.Extensions;
using Hapi.Common.DependencyInjections;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hapi.Client.Book.Extensions
{
    public static class DependencyBuilderExtension
    {
        public static DependencyBuilder AddBookClient(this DependencyBuilder builder)
        {
            builder.AddHapiClient();
            builder.Services.TryAddTransient<IBookClient, BookClient>();
            builder.Context.BuildServiceProvider();
            return builder;
        }
    }
}

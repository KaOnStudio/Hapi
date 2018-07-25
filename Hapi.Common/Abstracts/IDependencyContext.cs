using System;
using Microsoft.Extensions.DependencyInjection;

namespace Hapi.Common.Abstracts
{
    public interface IDependencyContext
    {
        IServiceCollection Services { get; }
        IServiceProvider ServiceProvider { get; }

        void BuildServiceProvider();
    }
}
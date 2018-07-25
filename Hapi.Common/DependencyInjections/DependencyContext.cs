using System;
using Hapi.Common.Abstracts;
using Microsoft.Extensions.DependencyInjection;
namespace Hapi.Common.DependencyInjections
{
    public class DependencyContext:IDependencyContext
    {
        public IServiceCollection Services { get; }
        public IServiceProvider ServiceProvider { get; private set; }

        public DependencyContext(IServiceCollection services)
        {
            Services = services;
        }
        public void BuildServiceProvider()
        {
            ServiceProvider = Services.BuildServiceProvider();
        }
    }
}

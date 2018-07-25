using Hapi.Common.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Hapi.Common.DependencyInjections
{
    public class DependencyBuilder
    {
        #region Properties

        public IServiceCollection Services { get; }
        public IDependencyContext Context { get; }

        #endregion

        public DependencyBuilder(IServiceCollection services)
        {
            Services = services;
            Context=new DependencyContext(Services);
        }
    }
}

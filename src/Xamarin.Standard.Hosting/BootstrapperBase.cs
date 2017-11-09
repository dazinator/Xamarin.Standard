using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Xamarin.Standard.Hosting
{
    public abstract class BootstrapperBase
    {

        public IServiceProvider BootstrapFromStartupClasses<TStartup>(IServiceCollection services, IServiceProvider activatorServiceProvider, Func<IServiceCollection, IServiceProvider> buildProvider)
              where TStartup : IStartup
        {

            var candidates = this.GetStartupTypes<TStartup>();

            var startupItems = new List<TStartup>();
            foreach (var item in candidates)
            {
                TStartup startupInstance;
                if (activatorServiceProvider != null)
                {
                    startupInstance = (TStartup)ActivatorUtilities.CreateInstance(activatorServiceProvider, item);
                }
                else
                {
                    startupInstance = (TStartup)Activator.CreateInstance(item);
                }

                startupItems.Add(startupInstance);
                startupInstance.RegisterServices(services);
            }

            var serviceProvider = buildProvider(services);
            foreach (var item in startupItems)
            {
                item.OnConfigured(serviceProvider);
            }

            return serviceProvider;
        }

        public abstract IEnumerable<Type> GetStartupTypes<TStartup>()
            where TStartup : IStartup;

    }

}



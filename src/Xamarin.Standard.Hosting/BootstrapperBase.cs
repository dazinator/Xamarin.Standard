using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Xamarin.Standard.Hosting
{
    public abstract class BootstrapperBase
    {

        protected virtual bool IsMatchForEnvironment(Type startupType, string name)
        {
            var atts = startupType.GetTypeInfo().GetCustomAttributes<EnvironmentNameAttribute>();
            if (!atts.Any())
            {
                return true;
            }

            foreach (var att in atts)
            {
                if (att.Name == name)
                {
                    return true;
                }
            }

            return false;

        }

        public IServiceProvider BootstrapFromStartupClasses<TStartup>(IServiceCollection services, IServiceProvider activator, Func<IServiceCollection, IServiceProvider> buildProvider, string environmentName, Predicate<AssemblyName> assemblyFilter = null)
              where TStartup : IStartup
        {

            var candidates = GetStartupTypes<TStartup>().Where(t => IsMatchForEnvironment(t, environmentName));

            var startupItems = new List<TStartup>();
            foreach (var item in candidates)
            {
                TStartup startupInstance;
                if (activator != null)
                {
                    startupInstance = (TStartup)ActivatorUtilities.CreateInstance(activator, item);
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

        public abstract IEnumerable<Type> GetStartupTypes<TStartup>(Predicate<AssemblyName> assemblyFilter = null)
            where TStartup : IStartup;

    }  

}
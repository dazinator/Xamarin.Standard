using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using Xamarin.Standard.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EnvironmentServiceCollectionExtensions
    {
        public static IServiceCollection AddEnvironments(this IServiceCollection services, Func<IServiceCollection, IServiceProvider> buildProviderDelegate, Action<EnvironmentManager> configureEnvironments)
        {
            services.AddSingleton<EnvironmentManager>((sp) =>
            {
                var envManager = new EnvironmentManager(buildProviderDelegate, sp);
                configureEnvironments(envManager);
                return envManager;
            });

            return services;
        }
        
        public static IHostingEnvironment GetEnvironment(this IServiceProvider parentServiceProvider, string environmentName)
        {
            var envManager = parentServiceProvider.GetRequiredService<EnvironmentManager>();
            return envManager.GetEnvironment(environmentName);
        }
        
        public static IServiceProvider Initialise<TStartup>(this IServiceCollection services, IEnumerable<Type> startupTypes, Func<IServiceCollection, IServiceProvider> buildProvider, IServiceProvider activator = null, string environmentName = null)
         where TStartup : IStartup
        {
            var bootstrapper = new EnumerableStartupClassesBootstrapper(startupTypes, environmentName);
            return bootstrapper.BootstrapFromStartupClasses<TStartup>(services, activator, buildProvider, environmentName);
        }
        
        public static IServiceCollection AddDefaultHostingEnvironment(this IServiceCollection services, string applicationName, string contentRootPath, string environmentName = "Default", IFileProvider contentFileProvider = null)
        {
            services.AddSingleton<IHostingEnvironment>((sp) =>
            {
                var hostEnv = new HostingEnvironment();
                hostEnv.EnvironmentName = environmentName;
                hostEnv.ApplicationName = applicationName;
                hostEnv.ContentRootPath = contentRootPath;

                if (contentFileProvider == null)
                {
                    contentFileProvider = new FileProviders.NullFileProvider();
                }
                hostEnv.ContentRootFileProvider = contentFileProvider;
                hostEnv.ServiceProvider = sp;
                return hostEnv;
            });
            return services;
        }
    }
}

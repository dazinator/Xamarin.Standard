using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Standard.Hosting
{
    public class EnvironmentManager
    {
        private ConcurrentDictionary<string, IHostingEnvironment> _environments;
        private readonly Func<IServiceCollection, IServiceProvider> _buildProviderDelegate;
        private readonly IServiceProvider _hostProvider;

        public IServiceProvider HostProvider
        {
            get => _hostProvider;
        }

        public EnvironmentManager(Func<IServiceCollection, IServiceProvider> buildProvider, IServiceProvider hostProvider)
        {
            _environments = new ConcurrentDictionary<string, IHostingEnvironment>();
            _buildProviderDelegate = buildProvider;
            _hostProvider = hostProvider;
        }

        public void ConfigureEnvironment(string name, Action<ServiceCollection> configure)
        {

            var hostEnvironment = HostProvider.GetRequiredService<IHostingEnvironment>();
            var newEnvironment = new HostingEnvironment() { ApplicationName = hostEnvironment.ApplicationName, ContentRootFileProvider = hostEnvironment.ContentRootFileProvider, ContentRootPath = hostEnvironment.ContentRootPath, EnvironmentName = name };
            _environments.AddOrUpdate(name, (key) =>
            {
                // initialise from named startup?
                var services = new ServiceCollection();
                services.AddSingleton<IHostingEnvironment>(newEnvironment);
                configure(services);
                //  services.AddAndroidAuthenticator(configure);
                var sp = _buildProviderDelegate(services);
                // var existingEnv = sp.GetService<IHostingEnvironment>();
                newEnvironment.ServiceProvider = sp;
                return newEnvironment;
            },
            (key, existing) =>
            {
                var services = new ServiceCollection();
                services.AddSingleton<IHostingEnvironment>(newEnvironment);
                configure(services);
                // services.AddAndroidAuthenticator(configure);
                var sp = _buildProviderDelegate(services);
                newEnvironment.ServiceProvider = sp;
                return newEnvironment;
            });
        }

        public IHostingEnvironment GetEnvironment(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            IHostingEnvironment result;
            _environments.TryGetValue(name, out result);
            return result;
        }

        public string GetFirstOrDefaultEnvironmentName()
        {
            var envName = _environments.Keys.FirstOrDefault();
            return envName;
        }

        public IHostingEnvironment GetFirstOrDefaultEnvironment()
        {
            var envName = GetFirstOrDefaultEnvironmentName();
            return GetEnvironment(envName);
        }

        public List<string> GetEnvironmentNames()
        {
            var results = _environments.Keys.ToList();
            return results;
        }

    }
}

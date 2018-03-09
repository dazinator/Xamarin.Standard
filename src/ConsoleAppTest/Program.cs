using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xamarin.Standard.Hosting;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddDefaultHostingEnvironment("ConsoleApp", Environment.CurrentDirectory);

            services.AddEnvironments((sp) => sp.BuildServiceProvider(), (envManager) =>
            {
                // Enumerable list of the types that are startup classes (i.e implement IStartup). Can be scanned from assemblies etc.
                var startupTypes = new List<Type>() { typeof(Startup), typeof(AnotherStartup) };

                envManager.ConfigureEnvironment("Dev", (envServices) =>
                {
                    // add dev environment specific services     

                    // populate services from startup classes..
                    envServices.Initialise<IStartup>(startupTypes, (sp) => sp.BuildServiceProvider(), envManager.HostProvider);
                });

                envManager.ConfigureEnvironment("Test", (envServices) =>
                {
                    // populate services from startup classes.. This time startup class with "Test" environment name attribute will be run..
                    envServices.Initialise<IStartup>(startupTypes, (sp) => sp.BuildServiceProvider(), envManager.HostProvider, "Test");

                });
            });

            var hostServiceProvider = services.BuildServiceProvider();

            var devEnvironment = hostServiceProvider.GetEnvironment("Dev");
            if (devEnvironment == null)
            {
                throw new Exception();
            }

            var registered = devEnvironment.ServiceProvider.GetRequiredService<Startup>();
            var notRegistered = devEnvironment.ServiceProvider.GetService<AnotherStartup>();
            if (notRegistered != null)
            {
                throw new Exception("Should only be registered in test environment as it has EnvironmentName attribute.");
            }

            var testEnvironment = hostServiceProvider.GetEnvironment("Test");
            if (testEnvironment == null)
            {
                throw new Exception();
            }

            registered = testEnvironment.ServiceProvider.GetRequiredService<Startup>();
            var alsoRegistered = testEnvironment.ServiceProvider.GetRequiredService<AnotherStartup>();


        }
    }



}


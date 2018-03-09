using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Standard.Hosting;
using Xamarin.Standard.Hosting.Android;

namespace Android.App
{
    public static class AndroidAppExtensions
    {        

        /// <summary>
        /// The entry point for bootstrapping the application on the android platform.
        /// </summary>
        /// <typeparam name="TStartup">The startup class to participate on configuring the IServiceProvider.</typeparam>
        /// <param name="application">The Android.App.Application.</param>
        /// <param name="IHostingEnvironment">The current environment - used to filter startup classes with an Environment attribute.</param>
        /// <param name=""></param>
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Application application, Func<IServiceCollection, IServiceProvider> buildProvider, IServiceProvider activator)
         where TStartup : IStartup
        {
            var services = new ServiceCollection();
            return Initialise<TStartup>(application, services, buildProvider, activator);
        }    

      
        /// <summary>
        /// Finds startup classes and uses them to populate a ServiceCollection, then builds the ServiceCollection to return an <see cref="IServiceProvider"/>
        /// </summary>
        /// <typeparam name="TStartup">The type that Startup classes should implement or derive from.
        /// <param name="application"></param> The android application.
        /// <param name="services"></param> The <see cref="ServiceCollection"/> that services will be registered into by the startup classes that are discovered.
        /// <param name="buildProvider"></param> A delegate that you can provide that we be called in order to obtain an <see cref="IServiceProvider"/> from an <see cref="IServiceCollection"/>. This is necessary if using a higher version of Microsoft.Extensions.DependendencyInjection than v1 due to breaking changes that Microsoft made.
        /// <param name="activator">An optional Service Provider that will be used to activate the startup classes - useful if you want to allow them to use DI./param>
        /// <param name="environmentName"></param> Will only invoke the startup classes with a matching <see cref="EnvironmentNameAttribute"/>. Useful if you want to bootstrap isolated <see cref="IServiceProvider"/> representing different environments.
        /// <param name="assemblyFilter"></param> A predicate that can be used to restrict which assemblies will be scanned for startup types. Useful for performance or security reasons, where you want to restrict the assemblies startup types can be discovered in to a subset.
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Application application, 
            IServiceCollection services, 
            Func<IServiceCollection, IServiceProvider> buildProvider,
            IServiceProvider activator = null,
            string environmentName = null,
            Predicate<AssemblyName> assemblyFilter = null)
         where TStartup : IStartup
        {         
            var bootstrapper = new AndroidBootstrapper();
            return bootstrapper.BootstrapFromStartupClasses<TStartup>(services, activator, buildProvider, environmentName, assemblyFilter);
        }         

     

        /// <summary>
        /// The entry point for bootstrapping an android Service on the android platform.
        /// </summary>
        /// <typeparam name="TStartup">The startup class to participate on configuring the IServiceProvider.</typeparam>       
        /// <param name="Service">The Android.App.Service to be bootstrapped.</param>        
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Service service, Func<IServiceCollection, IServiceProvider> buildProvider, IServiceProvider activator = null)
         where TStartup : IStartup
        {
            var services = new ServiceCollection();
            return Initialise<IStartup>(service, services, buildProvider, activator);
        }

        /// <summary>
        /// The entry point for bootstrapping an android Service on the android platform.
        /// </summary>
        /// <typeparam name="TStartup">The startup class to participate on configuring the IServiceProvider.</typeparam>       
        /// <param name="Service">The Android.App.Service to be bootstrapped.</param>        
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Service service, IServiceCollection services, Func<IServiceCollection, IServiceProvider> buildProvider, IServiceProvider activator = null, string environmentName = null, Predicate<AssemblyName> assemblyFilter = null)
         where TStartup : IStartup
        {           
            var bootstrapper = new AndroidBootstrapper();
            return bootstrapper.BootstrapFromStartupClasses<TStartup>(services, activator, buildProvider, environmentName, assemblyFilter);
        }

    }

}

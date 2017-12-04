using System;
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
        public static IServiceProvider Initialise<TStartup>(this Application application, IServiceProvider activator = null)
         where TStartup : IStartup
        {
            var services = new ServiceCollection();
            return Initialise<TStartup>(application, services, activator);
        }

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
        /// The entry point for bootstrapping the application on the android platform.
        /// </summary>
        /// <typeparam name="TStartup">The startup class to participate on configuring the IServiceProvider.</typeparam>
        /// <typeparam name="TApp">The Xamarin.Forms.Application to be resolved from the configured IServiceProvider.</typeparam>
        /// <param name="application">The Android.App.Application.</param>      
        /// <param name=""></param>
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Application application, IServiceCollection services, IServiceProvider activator)
         where TStartup : IStartup
        {
            return Initialise<TStartup>(application, services, (coll) => coll.BuildServiceProvider(), activator);
        }

        /// <summary>
        /// The entry point for bootstrapping the application on the android platform.
        /// </summary>
        /// <typeparam name="TStartup">The startup class to participate on configuring the IServiceProvider.</typeparam>
        /// <typeparam name="TApp">The Xamarin.Forms.Application to be resolved from the configured IServiceProvider.</typeparam>
        /// <param name="application">The Android.App.Application.</param>       
        /// <param name=""></param>
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Application application, IServiceCollection services, Func<IServiceCollection, IServiceProvider> buildProvider, IServiceProvider activator = null, string environmentName = null)
         where TStartup : IStartup
        {           
            var bootstrapper = new AndroidBootstrapper();
            return bootstrapper.BootstrapFromStartupClasses<TStartup>(services, activator, buildProvider, environmentName);
        }



        /// <summary>
        /// The entry point for bootstrapping an android Service on the android platform.
        /// </summary>
        /// <typeparam name="TStartup">The startup class to participate on configuring the IServiceProvider.</typeparam>       
        /// <param name="Service">The Android.App.Service to be bootstrapped.</param>        
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Service service, IServiceProvider activator = null)
         where TStartup : IStartup
        {
            var services = new ServiceCollection();
            return Initialise<IStartup>(service, services, activator);
        }

        /// <summary>
        /// The entry point for bootstrapping an android Service on the android platform.
        /// </summary>
        /// <typeparam name="TStartup">The startup class to participate on configuring the IServiceProvider.</typeparam>       
        /// <param name="Service">The Android.App.Service to be bootstrapped.</param>        
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Service service, IServiceCollection services, IServiceProvider activator = null)
         where TStartup : IStartup
        {
            return Initialise<IStartup>(service, services, (coll) => coll.BuildServiceProvider(), activator);
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
        public static IServiceProvider Initialise<TStartup>(this Service service, IServiceCollection services, Func<IServiceCollection, IServiceProvider> buildProvider, IServiceProvider activator = null, string environmentName = null)
         where TStartup : IStartup
        {           
            var bootstrapper = new AndroidBootstrapper();
            return bootstrapper.BootstrapFromStartupClasses<TStartup>(services, activator, buildProvider, environmentName);
        }

    }

}

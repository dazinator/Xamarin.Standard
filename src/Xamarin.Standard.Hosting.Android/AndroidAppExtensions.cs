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
        /// <typeparam name="TApp">The Xamarin.Forms.Application to be resolved from the configured IServiceProvider.</typeparam>
        /// <param name="application">The Android.App.Application.</param>
        /// <param name="context">The ContextWrapper for the MainAcitvity.</param>
        /// <param name=""></param>
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Application application, IServiceProvider activatorServiceProvider = null)
         where TStartup : IStartup
        {
            var services = new ServiceCollection();
            return Initialise<TStartup>(application, services, activatorServiceProvider);
        }

        /// <summary>
        /// The entry point for bootstrapping the application on the android platform.
        /// </summary>
        /// <typeparam name="TStartup">The startup class to participate on configuring the IServiceProvider.</typeparam>
        /// <typeparam name="TApp">The Xamarin.Forms.Application to be resolved from the configured IServiceProvider.</typeparam>
        /// <param name="application">The Android.App.Application.</param>
        /// <param name="context">The ContextWrapper for the MainAcitvity.</param>
        /// <param name=""></param>
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Application application, IServiceCollection services, IServiceProvider activatorServiceProvider = null)
         where TStartup : IStartup
        {
            services.AddAndroidHostingEnvironment();
            var bootstrapper = new AndroidBootstrapper();
            return bootstrapper.BootstrapFromStartupClasses<TStartup>(services, activatorServiceProvider);
        }

        /// <summary>
        /// Initialises the `IServiceProvider` by scanning for types that implment <see cref="IStartup"/> and activating them.
        /// </summary>
        /// <param name="application"></param>
        /// <param name="activatorServiceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider Initialise(this Application application, IServiceProvider activatorServiceProvider = null)
        {
            var services = new ServiceCollection();
            return Initialise(application, services, activatorServiceProvider);
        }

        /// <summary>
        /// Initialises the `IServiceProvider` by scanning for types that implment <see cref="IStartup"/> and activating them.
        /// </summary>
        /// <param name="application"></param>
        /// <param name="activatorServiceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider Initialise(this Application application, IServiceCollection services, IServiceProvider activatorServiceProvider = null)
        {
            return Initialise<IStartup>(application, services, activatorServiceProvider);
        }


        /// <summary>
        /// The entry point for bootstrapping an android Service on the android platform.
        /// </summary>
        /// <typeparam name="TStartup">The startup class to participate on configuring the IServiceProvider.</typeparam>       
        /// <param name="Service">The Android.App.Service to be bootstrapped.</param>        
        /// <param name="activatorServiceProvider">A service provider that can be used when activating instances of the <typeparamref name="TStartup"/> classes - if you want to inject them.</param>
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Service service, IServiceProvider activatorServiceProvider = null)
         where TStartup : IStartup
        {
            var services = new ServiceCollection();
            return Initialise<IStartup>(service, services, activatorServiceProvider);
        }

        /// <summary>
        /// The entry point for bootstrapping an android Service on the android platform.
        /// </summary>
        /// <typeparam name="TStartup">The startup class to participate on configuring the IServiceProvider.</typeparam>       
        /// <param name="Service">The Android.App.Service to be bootstrapped.</param>        
        /// <param name="activatorServiceProvider">A service provider that can be used when activating instances of the <typeparamref name="TStartup"/> classes - if you want to inject them.</param>
        /// <returns></returns>
        public static IServiceProvider Initialise<TStartup>(this Service service, IServiceCollection services, IServiceProvider activatorServiceProvider = null)
         where TStartup : IStartup
        {
            services.AddAndroidHostingEnvironment();
            var bootstrapper = new AndroidBootstrapper();
            return bootstrapper.BootstrapFromStartupClasses<TStartup>(services, activatorServiceProvider);
        }

        /// <summary>
        /// Initialises the `IServiceProvider` by scanning for types that implment <see cref="IStartup"/> and activating them.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="activatorServiceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider Initialise(this Service service, IServiceProvider activatorServiceProvider = null)
        {
            var services = new ServiceCollection();
            return Initialise(service, services, activatorServiceProvider);
        }

        /// <summary>
        /// Initialises the `IServiceProvider` by scanning for types that implment <see cref="IStartup"/> and activating them.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="activatorServiceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider Initialise(this Service service, IServiceCollection services, IServiceProvider activatorServiceProvider = null)
        {
            return Initialise<IStartup>(service, services, activatorServiceProvider);
        }

    }

}

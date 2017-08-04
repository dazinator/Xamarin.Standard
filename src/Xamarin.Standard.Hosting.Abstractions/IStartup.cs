using Microsoft.Extensions.DependencyInjection;
using System;


namespace Xamarin.Standard.Hosting
{
    /// <summary>
    ///  Implement this to contract in order to participate in service registration into the main appliction container.
    /// </summary>
    public interface IStartup
    {
        void RegisterServices(IServiceCollection services);

        /// <summary>
        /// Called after the container has been built, and is a good time to perform initial startup actions such as db migrations etc.
        /// </summary>
        /// <param name="provider"></param>
        void OnConfigured(IServiceProvider provider);
    }
}


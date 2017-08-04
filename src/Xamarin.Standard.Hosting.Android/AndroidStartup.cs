using System;
using Android.Content;
using Microsoft.Extensions.DependencyInjection;
using Android.App;

namespace Xamarin.Standard.Hosting.Android
{
    public abstract class AndroidStartup : IStartup
    {
        private readonly AppContextProvider _appContextProvider;

        public AndroidStartup(AppContextProvider appContextProvider)
        {
            _appContextProvider = appContextProvider;
        }

        protected Context Context { get { return _appContextProvider.CurrentContext; } }

        public virtual void RegisterServices(IServiceCollection services)
        {

        }

        public virtual void OnConfigured(IServiceProvider provider)
        {

        }
    }
}
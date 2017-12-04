using Android.App;
using Xamarin.Standard.Hosting;
using Xamarin.Standard.Hosting.Android;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAndroidHostingEnvironment(this IServiceCollection services, string environmentName = "Default")
        {
            var appContext = AppContextProvider.DefaultAppContextProvider;
            services.AddSingleton<AppContextProvider>(appContext);          
           
            var context = appContext.CurrentContext;
            var appName = context.ApplicationInfo.LoadLabel(context.PackageManager);
            var hostEnv = new AndroidHostingEnvironment(appName);
            hostEnv.EnvironmentName = environmentName;
            services.AddSingleton<IHostingEnvironment>(hostEnv);
            return services;
        }
    }
}



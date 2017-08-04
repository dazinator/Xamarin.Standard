using Android.App;
using Xamarin.Standard.Hosting;
using Xamarin.Standard.Hosting.Android;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAndroidHostingEnvironment(this IServiceCollection services)
        {
            var appContext = AppContextProvider.DefaultAppContextProvider;
            services.AddSingleton<AppContextProvider>(appContext);
            var context = appContext.CurrentContext;
            var appName = context.ApplicationInfo.LoadLabel(context.PackageManager);
            var hostEnv = new AndroidHostingEnvironment(appName);
            services.AddSingleton<IHostingEnvironment>(hostEnv);
        }
    }
}



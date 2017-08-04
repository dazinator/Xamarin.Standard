using Android.App;
using Microsoft.Extensions.DependencyInjection;
using Android.Content;

namespace Todo
{
    public interface IAndroidModule
    {
        void RegisterServices(IServiceCollection services, Application application, ContextWrapper context);
    }
}



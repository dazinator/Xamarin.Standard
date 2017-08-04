using Android.App;
using Microsoft.Extensions.DependencyInjection;
using Android.Content;
using System;

namespace Todo
{
    public abstract class AndroidModule : IAndroidModule
    {
        public abstract void RegisterServices(IServiceCollection services, Application application, ContextWrapper context);
       
    }
}



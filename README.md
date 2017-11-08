
## Bootstrap your `netstandard` based xamarin applications.


### Setup

1. You should have your various platform specific / native xamarin projects.
2. You should have your cross platform logic in `netstandard` projects.
2. In `netstandard` project, add nuget package: `Xamarin.Standard.Hosting.Abstractions`
3. In your `Android` project, add nuget package: `Xamarin.Standard.Hosting.Android`

### Startup classes

You can create a startup class in both your `netstandard` and your platform (android) specific projects.

For `netstandard` project:

```
using Xamarin.Standard.Hosting;

namespace Todo
{
    publi class MyStartup: IStartup
    {
	   /// <summary>
           /// Called on application startup, register any services here..
           /// </summary>
           /// <param name="provider"></param>
	   public void RegisterServices(IServiceCollection services)
	   {
	          // e.g:
	          // Register services here.
                  services.AddEntityFrameworkSqlite();
                  services.AddDbContext<TodoItemDatabase>();

		   // You can inject into your Xamarin `App` class if you register it in configure services. You don't have too!
		   services.AddTransient<App>();     
	   }

       /// <summary>
       /// Called after the container has been built, and is a good time to perform initial startup actions such migrations etc.
       /// </summary>
       /// <param name="provider"></param>
       public void OnConfigured(IServiceProvider provider)
	   {
		    using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TodoItemDatabase>();
                db.Database.EnsureCreated();
                db.Database.Migrate();
            }
	   }
    }
}

```

You can also create a `startup` class in your platform (`Android`) project if you need to register platform specific services.
Either implement `IStartup` or derive from `Xamarin.Standard.Hosting.Android.AndroidStartup`.
The `AndroidStartup` class is special becuase it has access to the current Android `Context` which is often needed when dealing with services on the Android platform.


## Running the startup classes (Android)

To bootstrap your application you can call the `Initialise` extension method. In your Android Main Activity:

```

[Activity(Label = "Todo", Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        private IServiceProvider _serviceProvider;

        protected override void OnCreate(Bundle bundle)
        {
			_serviceProvider = MainApp.Current.Initialise<IStartup>();    // your startup classes will be run.    

			base.OnCreate(bundle);

			// init xamarin forms etc.
			Forms.Init(this, bundle);

			// You can inject into your Xamarin `App` here if you have registered it in configure services.
			// otherwise just use new App();
			var app = _serviceProvider.Value.GetRequiredService<App>();
			LoadApplication(app);

		}
    }
```

This will register all services into an `IServiceCollection` and then build a default `IServiceProvider` and return it.

If you have an existing container that you wish to populate with the services instead, then you can do this as long as the container supports `Microsoft.Extensions.DependencyInjection`:


```
// Pass an existing ServiceCollection() in so you can capture the services added by all startup classes.
var services = new ServiceCollection();
MainApp.Current.Initialise<IStartup>(services);

// Now add these services to your existing container.
existingContainer.Populate(services);

```

In the example above, we allow all our startup classes to populate a `ServiceCollection` which we then populate our existing container of choice with. For example, if you are using `Prism` you will want to do this to populate the container that prism creates for you.

# IHostingEnvironment

An `IHostingEnvironment` is aautomatically registered as a service. This means you can inject `IHostingEnvironment` anywhere you need it.
It has familiar properties like `ApplicationName`, `EnvironmentName` as well as `ContentRootPath` and `ContentFileProvider`.
ContentRootPath points to your applications path on disk, and the `IFileProvider` is provided so you can have read access to files and directories within your apps content folder.
This is very similar to `IHostingEnvironment` in asp.net core applications.


## Prism (Autofac)
With prism, we need to bootstrap into an existing container, whose lifetime is controlled by prism.
Here is an autofac example although other containers would be similar.

In your android project, create an `IPlatformInitialiser`:

```

using Autofac;
using Prism.Autofac.Forms;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Android.App;
using Xamarin.Standard.Hosting;

namespace Todo
{

    public class AndroidInitializer : IPlatformInitializer
    {
        private readonly Android.App.Application _application;

        public AndroidInitializer(Android.App.Application application)
        {
            _application = application;
        }

        public void RegisterTypes(IContainer container)
        {
            IServiceCollection services = new ServiceCollection();
            _application.Initialise<IStartup>(services);

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            containerBuilder.Update(container);
        }
    }
}

```

In your `MainActivty` when you create the prism application, pass in the `AndroidInitializer`:

```

 CurrentApp = new App(new AndroidInitializer(MainApp.Current));

```

Where `MainApp.Current` returns the `Android.App.Application` instance. You can implement `MainApp` like this:

```

using System;
using Android.App;
using Android.Runtime;

namespace Todo
{
    [Application]
    public class MainApp : Application
    {

        private static MainApp _current;

        public MainApp(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {

        }

        public override void OnCreate()
        {
            try
            {
                base.OnCreate();

                // Application Initialisation ...
                _current = this;

                // Global error handling etc.
                //   AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
                //   AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            }
            catch (Exception e)
            {
                // Log(e);
                throw;
            }
        }

        public static MainApp Current
        {
            get { return _current; }
        }

    }
}

```
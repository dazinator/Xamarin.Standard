1. Create a new xamarin project, with your core library targeting netstandard.
2. In your core library add nuget package: `Xamarin.Standard.Hosting.Abstractions`
3. In your android platform project add nuget package: `Xamarin.Standard.Hosting.Android`
4. In your core library project create a startup class:

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

5. In your Android Main Activity 

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

You can also create a `startup` class in your platform (`Android`) project if you wals need to register platform specific services.
Just derive from `Xamarin.Standard.Hosting.Android.AndroidStartup`.
The `AndroidStartup` class is special becuase it has access to the current Android `Context` which is often needed when dealing with services on the Android platform.

You can register pages / view models etc in the container so you can resolve them later. 
If you would prefer for the servcies to be added to another / existing container (i.e Unity, StructureMap etc) then you can do that as long as the containers are compatible with `Microsoft.Extensions.DependencyInjection`:

```
// Pass an existing ServiceCollection() in so you can capture the services added by all startup classes.
var services = new ServiceCollection();
var serviceProvider = MainApp.Current.Initialise<IStartup>(services); // the service provider is kind of redundant as we want to use a different container to resolve with i.e (unity, structuremap etc)

existingContainer.Populate(services);

```

In the example above, we allow all our startup classes to populate a `ServiceCollection` which we then populate our existing container of choice with. For example, if you are using `Prism` you will want to do this to populate the container that prism creates for you.

# IHostingEnvironment

An `IHostingEnvironment` is added to the container for you. This means you can inject `IHostingEnvironment` anywhere you need it.
It has familiar properties like `ApplicationName`, `EnvironmentName` as well as `ContentRootPath` and `ContentFileProvider`.
ContentRootPath points to your applications path on disk, and the `IFileProvider` is provided so you can have read access to files and directories within your apps content folder.
This is very similar to `IHostingEnvironment` in asp.net core applications.


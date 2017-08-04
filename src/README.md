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
            _serviceProvider = MainApp.Current.Initialise<IStartup>();        

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

You can register pages / view models etc in the container so you can resolve them later. 
You will probably want to use a proper `mvvm` framework though (INavigationService etc) so you will probably need to integrate these services with an existing container.
You can do this with containers that are compatible with `Microsoft.Extensions.DependencyInjection` by:

```

var services = new ServiceCollection();
var serviceProvider = MainApp.Current.Initialise<IStartup>(services); 
// the service provider is kind of redundant as we want to use a different container (unity, structuremap etc)
existingContainer.Populate(services);

```

# IHostingEnvironment

An `IHostingEnvironment` is added to the container for you. This means you can inject `IHostingEnvironment` anywhere you need it.
It has familiar properties like `ApplicationName`, `EnvironmentName` as well as `ContentRootPath` and `ContentFileProvider`.
ContentRootPath points to your applications path on disk, and the `IFileProvider` is provided so you can have read access to files and directories within your apps content folder.
This is very similar to `IHostingEnvironment` in asp.net core applications.


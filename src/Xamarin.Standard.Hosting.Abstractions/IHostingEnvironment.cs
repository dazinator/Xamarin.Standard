using Microsoft.Extensions.FileProviders;

namespace Xamarin.Standard.Hosting
{
    //
    // Summary:
    //     Provides information about the web hosting environment an application is running
    //     in.
    public interface IHostingEnvironment
    {
        //
        // Summary:
        //     Gets or sets the name of the environment. 
        string EnvironmentName { get; set; }
        //
        // Summary:
        //     Gets or sets the name of the application. 
        string ApplicationName { get; set; }     
     
        string ContentRootPath { get; set; }
        //
        // Summary:
        //     Gets or sets an Microsoft.Extensions.FileProviders.IFileProvider pointing at
        //     IHostingEnvironment.ContentRootPath.
        IFileProvider ContentRootFileProvider { get; set; }
    }
}

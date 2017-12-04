using Xamarin.Standard.FileProviders;

namespace Xamarin.Standard.Hosting.Android
{
    public class AndroidHostingEnvironment : HostingEnvironment
    {
        public AndroidHostingEnvironment(string appName) : this(appName, System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal))
        {

        }

        public AndroidHostingEnvironment(string appName, string contentRootPath)
        {
            this.ContentRootPath = contentRootPath;
            var fp = new NonWatchingPhysicalFileProvider(contentRootPath);
            this.ContentRootFileProvider = fp;          
            this.ApplicationName = appName;          
        }

    }
}



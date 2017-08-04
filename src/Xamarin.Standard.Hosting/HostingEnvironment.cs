using Microsoft.Extensions.FileProviders;

namespace Xamarin.Standard.Hosting
{

    public class HostingEnvironment : IHostingEnvironment
    {
        public HostingEnvironment()
        {

        }
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }      
     
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }

    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Standard.Hosting;

namespace ConsoleAppTest
{
    [EnvironmentName("Test")] // Will only apply for Test environment.
    public class AnotherStartup : IStartup
    {
        public bool Fired { get; set; }

        public void OnConfigured(IServiceProvider provider)
        {
            Fired = true;
            Console.WriteLine("Configured!");
        }

        public void RegisterServices(IServiceCollection services)
        {
            Fired = true;
            Console.WriteLine("Registering Services!");

        }
    }



}


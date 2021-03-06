﻿using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Standard.Hosting;

namespace ConsoleAppTest
{
    public class Startup : IStartup
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
            services.AddSingleton(this);
            Console.WriteLine("Registering Services!");

        }
    }



}


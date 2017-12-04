using System;

namespace Xamarin.Standard.Hosting
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EnvironmentNameAttribute : Attribute
    {    
        public EnvironmentNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

}



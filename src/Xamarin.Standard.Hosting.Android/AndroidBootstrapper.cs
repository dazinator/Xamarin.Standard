using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace Xamarin.Standard.Hosting.Android
{
    public class AndroidBootstrapper : BootstrapperBase
    {
        public override IEnumerable<Type> GetStartupTypes<TStartup>(Predicate<AssemblyName> assemblyFilter = null)
        {
            var startupType = typeof(TStartup);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (assemblyFilter != null)
            {
                assemblies = assemblies.Where((a) => assemblyFilter(a.GetName())).ToArray();
            }

            var candidates = new List<Type>();
            foreach (var item in assemblies)
            {              
                try
                {
                    var startupTypes = item.GetTypes().Where(p => startupType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract && p != startupType);
                    candidates.AddRange(startupTypes);
                }
                catch (ReflectionTypeLoadException e)
                {
                    Console.WriteLine(item.FullName);
                    Trace.WriteLine(item.FullName);
                    foreach (var type in e.Types)
                    {
                        if (type != null)
                        {
                            Trace.WriteLine(type.ToString());
                            //Console.WriteLine(type.ToString());
                        }
                        else
                        {
                            Trace.WriteLine("MISSING TYPE");
                            //Console.WriteLine("MISSING TYPE");
                        }

                    }
                    // skip this asssembly..
                    // throw;
                }
            }

            return candidates;

        }
    }

}



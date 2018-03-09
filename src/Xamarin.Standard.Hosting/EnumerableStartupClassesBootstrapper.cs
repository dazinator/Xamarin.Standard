using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xamarin.Standard.Hosting
{
    public class EnumerableStartupClassesBootstrapper : BootstrapperBase
    {
        private IServiceProvider _sp;
        private IEnumerable<Type> _startupTypes;

        private readonly string _environmentName;

        public EnumerableStartupClassesBootstrapper(IEnumerable<Type> startupTypes, string environmentName)
        {
            _startupTypes = startupTypes;
            _environmentName = environmentName;
        }

        public override IEnumerable<Type> GetStartupTypes<TStartup>(Predicate<AssemblyName> assemblyFilter = null)
        {
            var candidates = _startupTypes;
            if (!string.IsNullOrWhiteSpace(_environmentName))
            {
                // filter for environment.
                candidates = candidates.Where(a => IsMatchForEnvironment(a, _environmentName));
            }

            if (assemblyFilter != null)
            {
                candidates = candidates.Where((t)=> assemblyFilter(t.Assembly.GetName()));
            }

            return candidates;
        }
    }
}


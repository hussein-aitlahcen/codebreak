using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Configuration
{
    public class ConfigurationManager
    {
        private readonly IList<IConfigurationProvider> _providers;
        private readonly IList<ICommitableProvider> _commitableProviders; 
        private readonly IDictionary<string, FieldInfo> _configurables;

        public ConfigurationManager()
        {
            _providers = new List<IConfigurationProvider>();
            _commitableProviders = new List<ICommitableProvider>();
            _configurables = new Dictionary<string, FieldInfo>();
        }

        public bool TryGet(string key, out object value)
        {
            if (key == null) throw new ArgumentNullException("key");

            foreach (var provider in _providers.Reverse())
            {
                if (provider.TryGet(key, out value))
                {
                    return true;
                }
            }
            value = null;
            return false;
        }


        public void Set(string key, object value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");

            foreach (var provider in _providers)
            {
                provider.Set(key, value);
            }
        }

        public void RegisterAttributes()
        {
            RegisterAttributes(Assembly.GetCallingAssembly());
        }

        public void RegisterAttributes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            foreach (var type in assembly.GetTypes())
            {
                foreach (var field in type.GetFields())
                {
                    var attr = field.GetCustomAttribute<ConfigurableAttribute>();

                    if(attr == null)
                        continue;

                    if (_configurables.ContainsKey(attr.Name))
                        throw new Exception(string.Format("Configurable name's `{0}` is already used.", attr.Name));

                    _configurables.Add(attr.Name, field);
                }
            }
        }

        public void Load()
        {
            foreach (var configurable in _configurables)
            {
               object value;
                if (TryGet(configurable.Key, out value))
                {
                    configurable.Value.SetValue(null, value);
                }
            }
        }

        public void Commit()
        {
            var final = _commitableProviders.LastOrDefault();

            if (final == null)
                throw new InvalidOperationException("no commitable provider available");


            final.Commit();
        }


        public void Add(IConfigurationProvider configurationProvider, bool setAll = false)
        {
            if (setAll)
            {
                foreach (var configurable in _configurables)
                {
                    configurationProvider.Set(configurable.Key, configurable.Value.GetValue(null));
                }
            }

            configurationProvider.Load();
            _providers.Add(configurationProvider);

            if (configurationProvider is ICommitableProvider)
            {
                _commitableProviders.Add(configurationProvider as ICommitableProvider);
            }
        }
    }
}

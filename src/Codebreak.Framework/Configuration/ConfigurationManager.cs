using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationManager
    {
        private readonly IList<IConfigurationProvider> m_providers;
        private readonly IList<ICommitableProvider> m_commitableProviders; 
        private readonly IDictionary<string, FieldInfo> m_configurables;

        /// <summary>
        /// 
        /// </summary>
        public ConfigurationManager()
        {
            m_providers = new List<IConfigurationProvider>();
            m_commitableProviders = new List<ICommitableProvider>();
            m_configurables = new Dictionary<string, FieldInfo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(string key, out object value)
        {
            if (key == null) throw new ArgumentNullException("key");

            foreach (var provider in m_providers.Reverse())            
                if (provider.TryGet(key, out value))                
                    return true;
                            
            value = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");

            foreach (var provider in m_providers)
            {
                provider.Set(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RegisterAttributes()
        {
            RegisterAttributes(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
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

                    if (m_configurables.ContainsKey(attr.Name))
                        throw new Exception(string.Format("Configurable name's `{0}` is already used.", attr.Name));

                    m_configurables.Add(attr.Name, field);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Load()
        {
            foreach (var configurable in m_configurables)
            {
               object value;
                if (TryGet(configurable.Key, out value))
                {
                    configurable.Value.SetValue(null, value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Commit()
        {
            var final = m_commitableProviders.LastOrDefault();

            if (final == null)
                throw new InvalidOperationException("no commitable provider available");


            final.Commit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationProvider"></param>
        /// <param name="setAll"></param>
        public void Add(IConfigurationProvider configurationProvider, bool setAll = false)
        {
            if (setAll)
            {
                foreach (var configurable in m_configurables)
                {
                    configurationProvider.Set(configurable.Key, configurable.Value.GetValue(null));
                }
            }

            configurationProvider.Load();
            m_providers.Add(configurationProvider);

            if (configurationProvider is ICommitableProvider)
            {
                m_commitableProviders.Add(configurationProvider as ICommitableProvider);
            }
        }
    }
}

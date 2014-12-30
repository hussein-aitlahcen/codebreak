using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Codebreak.Framework.Configuration.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonConfigurationProvider : IConfigurationProvider, ICommitableProvider
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, object> m_entries = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        public string Path
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public JsonConfigurationProvider(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("path");

            Path = path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(string key, out object value)
        {
            if (!m_entries.ContainsKey(key))
            {
                value = null;
                return false;
            }

            value = m_entries[key];
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            if (!m_entries.ContainsKey(key))
                m_entries.Add(key, value);
            else
                m_entries[key] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canCreate"></param>
        public void Load(bool canCreate = true)
        {

            if (File.Exists(Path))
            {
                using (var inputStream = new FileStream(Path, FileMode.Open))
                {
                    var reader = new JsonTextReader(new StreamReader(inputStream));
                    var serializer = new JsonSerializer();
                    var entries = new Dictionary<string, object>();
                    reader.Read();
                    reader.Read(); // ??
                    while (reader.TokenType == JsonToken.PropertyName)
                    {
                        string propertyName = reader.Value as string;
                        reader.Read();
                        
                        object value;
                        if (reader.TokenType == JsonToken.Integer)
                            value = Convert.ToInt32(reader.Value);
                        else
                            value = serializer.Deserialize(reader);
                        entries.Add(propertyName, value);

                        reader.Read();
                    }

                    m_entries = entries;

                }
            }
            else
            {
                if(canCreate)
                    Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Commit()
        {
            var file = new FileStream(Path, FileMode.Create);

            try
            {
                GenerateFile(file);
            }
            catch (Exception)
            {
                file.Dispose();

                if (File.Exists(Path))
                    File.Delete(Path);

                throw;

            }
            finally
            {
                file.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputStream"></param>
        internal void GenerateFile(Stream outputStream)
        {
            var outputWriter = new JsonTextWriter(new StreamWriter(outputStream));
            outputWriter.Formatting = Formatting.Indented;

            outputWriter.WriteStartObject();
            foreach (var entry in m_entries)
            {
                outputWriter.WritePropertyName(entry.Key);
                outputWriter.WriteValue(entry.Value);
            }
            outputWriter.WriteEndObject();

            outputWriter.Flush();
        }
    }
}

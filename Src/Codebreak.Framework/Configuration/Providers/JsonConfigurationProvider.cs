using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Codebreak.Framework.Configuration.Providers
{
    public class JsonConfigurationProvider : IConfigurationProvider, ICommitableProvider
    {
        private Dictionary<string, object> _entries = new Dictionary<string, object>();

        public string Path
        {
            get;
            private set;
        }

        public JsonConfigurationProvider(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("path");

            Path = path;
        }

        public bool TryGet(string key, out object value)
        {
            if (!_entries.ContainsKey(key))
            {
                value = null;
                return false;
            }

            value = _entries[key];
            return true;
        }

        public void Set(string key, object value)
        {
            if (!_entries.ContainsKey(key))
                _entries.Add(key, value);
            else
                _entries[key] = value;
        }

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

                    _entries = entries;

                }
            }
            else
            {
                if(canCreate)
                    Commit();
            }
        }

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

        internal void GenerateFile(Stream outputStream)
        {
            var outputWriter = new JsonTextWriter(new StreamWriter(outputStream));
            outputWriter.Formatting = Formatting.Indented;

            outputWriter.WriteStartObject();
            foreach (var entry in _entries)
            {
                outputWriter.WritePropertyName(entry.Key);
                outputWriter.WriteValue(entry.Value);
            }
            outputWriter.WriteEndObject();

            outputWriter.Flush();
        }
    }
}

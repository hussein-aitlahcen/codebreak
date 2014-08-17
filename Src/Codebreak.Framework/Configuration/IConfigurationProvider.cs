namespace Codebreak.Framework.Configuration
{
    public interface IConfigurationProvider
    {
        bool TryGet(string key, out object value);

        void Set(string key, object value);

        void Load(bool canCreate = true);
    }
}

using Microsoft.Extensions.Configuration;

namespace Redstone.Configuration.Yaml.Internal
{
    internal class YamlConfigurationSource : FileConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();

            return new YamlConfigurationProvider(this);
        }
    }
}

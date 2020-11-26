using Microsoft.Extensions.Configuration;
using System.IO;

namespace Redstone.Configuration.Yaml.Internal
{
    internal class YamlConfigurationProvider : FileConfigurationProvider
    {
        public YamlConfigurationProvider(YamlConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            var parser = new YamlConfigurationFileParser();

            Data = parser.Parse(stream);
        }
    }
}

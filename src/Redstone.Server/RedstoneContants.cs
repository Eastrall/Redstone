using System;
using System.IO;
using System.Reflection;

namespace Redstone.Server
{
    public static class RedstoneContants
    {
        public const string DefaultFaviconFileName = "Redstone.Server.Resources.redstone.png";

        private static string _defaultFavicon;

        /// <summary>
        /// Gets the default project favicon.
        /// </summary>
        /// <returns>The default favicon as a Base64 string.</returns>
        public static string GetDefaultFavicon()
        {
            if (string.IsNullOrEmpty(_defaultFavicon))
            {
                using Stream resourceFileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DefaultFaviconFileName);

                if (resourceFileStream is not null)
                {
                    var buffer = new byte[resourceFileStream.Length];

                    resourceFileStream.Read(buffer, 0, buffer.Length);

                    _defaultFavicon = $"data:image/png;base64,{Convert.ToBase64String(buffer).Replace("\n", "")}";
                }
            }

            return _defaultFavicon;
        }
    }
}

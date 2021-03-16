using System.IO;

namespace Redstone.Common.IO
{
    public static class DirectoryUtilities
    {
        /// <summary>
        /// Creates a directory at the given if it doesn't exists.
        /// </summary>
        /// <param name="path">Path directory to create.</param>
        public static void CreateDirectoryIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}

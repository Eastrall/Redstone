using Redstone.Common.IO;
using System.IO;
using Xunit;

namespace Redstone.Common.Tests.IO
{
    public class DirectoryUtilitiesTest
    {
        [Fact]
        public void CreateDirectoryIfNotExistTest()
        {
            const string pathName = "data";
            DirectoryUtilities.CreateDirectoryIfNotExist(pathName);

            Assert.True(Directory.Exists(pathName));
        }
    }
}

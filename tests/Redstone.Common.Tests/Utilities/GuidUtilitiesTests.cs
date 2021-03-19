using Redstone.Common.Utilities;
using System;
using System.Text;
using Xunit;

namespace Redstone.Common.Tests.Utilities
{
    public class GuidUtilitiesTests
    {
        private static readonly string TextToTransform = "test";
        private static readonly Guid ExpectedGuid = new("098f6bcd-4621-3373-8ade-4e832627b4f6");

        [Fact]
        public void GenerateGuidFromBytesTest()
        {
            Guid generatedGuid = GuidUtilities.GenerateGuidFromBytes(Encoding.UTF8.GetBytes(TextToTransform));

            Assert.Equal(ExpectedGuid, generatedGuid);
        }

        [Fact]
        public void GenerateGuidFromStringTest()
        {
            Guid generatedGuid = GuidUtilities.GenerateGuidFromString(TextToTransform);

            Assert.Equal(ExpectedGuid, generatedGuid);
        }
    }
}

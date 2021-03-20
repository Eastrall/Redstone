using Redstone.Common.Extensions;
using Xunit;

namespace Redstone.Common.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("Hello world")]
        [InlineData("Hello_world")]
        [InlineData("Hello-world")]
        public void StringToPascalCaseTest(string value)
        {
            const string expectedValue = "HelloWorld";

            Assert.Equal(expectedValue, value.ToPascalCase());
        }

        [Fact]
        public void StringTakeCharactersTest()
        {
            const string value = "Hello world!";

            Assert.Equal("Hello", value.TakeCharacters(5));
            Assert.Equal("Hello w", value.TakeCharacters(7));
            Assert.Equal(value, value.TakeCharacters(value.Length + 1));
        }
    }
}

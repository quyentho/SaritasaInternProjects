using FluentAssertions;
using Xunit;

namespace RestaurantCheck.Test
{
    public class CheckItemFactoryTest
    {
        ICheckItem result;
        [Fact]
        public void Create_InputExitCode_ReturnsExitCheckItem()
        {
            string input = "x";

            result = CheckItemFactory.Create(input);

            result.Should().BeAssignableTo<ExitCheckItem>();
        }

        [Fact]
        public void Create_InputEmptyString_ReturnsEmptyCheckItem()
        {
            string input = string.Empty;

            result = CheckItemFactory.Create(input);

            result.Should().BeAssignableTo<EmptyCheckItem>();
        }

        [Theory]
        [InlineData("test,1,2,3")]
        [InlineData("test")]
        public void Create_InputInvalidFormat_ReturnsInvalidCheckItem(string input)
        {
            result = CheckItemFactory.Create(input);

            result.Should().BeAssignableTo<InvalidCheckItem>();
        }

        [Fact]
        public void Create_ValidInput_ReturnsCheckItem()
        {
            string input = "test,10";

            result = CheckItemFactory.Create(input);

            result.Should().BeAssignableTo<CheckItem>();
        }
    }
}

using FluentAssertions;
using Xunit;

namespace MarketDesk.Test
{
    public class OrderItemFactoryTest
    {
        IOrderItem result;
        [Fact]
        public void Create_InputExitCode_ReturnsExitOrderItem()
        {
            string input = "x";

            result = OrderItemFactory.Create(input);

            result.Should().BeAssignableTo<ExitOrderItem>();
        }

        [Fact]
        public void Create_InputEmptyString_ReturnsEmptyOrderItem()
        {
            string input = string.Empty;

            result = OrderItemFactory.Create(input);

            result.Should().BeAssignableTo<EmptyOrderItem>();
        }

        [Theory]
        [InlineData("test,1,2,3")]
        [InlineData("test")]
        public void Create_InputInvalidFormat_ReturnsInvalidOrderItem(string input)
        {
            result = OrderItemFactory.Create(input);

            result.Should().BeAssignableTo<InvalidOrderItem>();
        }

        [Fact]
        public void Create_ValidInput_ReturnsInvalidOrderItem()
        {
            string input = "test,10,2";

            result = OrderItemFactory.Create(input);

            result.Should().BeAssignableTo<OrderItem>();
        }
    }
}

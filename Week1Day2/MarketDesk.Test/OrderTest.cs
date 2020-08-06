using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace MarketDesk.Test
{
    public class OrderTest
    {
        private Order sut;
        public OrderTest()
        {
            sut = new Order();
        }

        [Fact]
        public void IsEmpty_EmptyItems_ReturnsTrue()
        {
            bool result = sut.IsEmpty();

            result.Should().BeTrue();
        }

        [Fact]
        public void IsEmpty_NotEmptyItems_ReturnsTrue()
        {
            string input = "food,10,1";
            sut.Items.Add(new OrderItem(input));

            bool result = sut.IsEmpty();

            result.Should().BeFalse();
        }

        [Fact]
        public void Calculate_ValidItems_ReturnsExactOrderResult()
        {
            string input = "food, 5, 20";
            sut.Items.Add(new OrderItem(input));

            OrderResult result = sut.Calculate();

            result.Should().Match<OrderResult>(r => r.Total == 100 && r.TotalWithTax == 103);
        }
    }
}

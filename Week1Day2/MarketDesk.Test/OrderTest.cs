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
            string input = "Vodka,10,1.0";
            string input2 = "Beer,3,6.0";
            string input3 = "Fish,1,10";
            sut.Items.Add(new OrderItem(input));
            sut.Items.Add(new OrderItem(input2));
            sut.Items.Add(new OrderItem(input3));

            OrderResult result = sut.Calculate();

            result.Should().Match<OrderResult>(r => r.Total == 38 && r.TotalWithTax == 39.14);
        }
    }
}

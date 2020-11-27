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
            sut.Items.Add(new OrderItem() {  Name = "food", Price = 10, Quantity = 5});

            bool result = sut.IsEmpty();

            result.Should().BeFalse();
        }

        [Fact]
        public void Calculate_ValidItems_ReturnsExactOrderResult()
        {
            sut.Items.Add(new OrderItem() { Name= "Vodka", Quantity = 10, Price = 1 });
            sut.Items.Add(new OrderItem() { Name= "Beer", Quantity = 3, Price = 6 });
            sut.Items.Add(new OrderItem() { Name= "Fish", Quantity = 1, Price = 10 });

            OrderResult result = sut.Calculate();

            result.Should().Match<OrderResult>(r => r.Total == 38 && r.TotalWithTax == 39.14);
        }
    }
}

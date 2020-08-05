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
        public void CalculateToTalPrice_ValidItems_ReturnsExactTotal()
        {
            sut.Items = new List<OrderItem>()
            {
                new OrderItem(){ Name = "Item1", Price = 100, Quantity = 5},
                new OrderItem(){ Name = "Item2", Price = 200, Quantity = 2},
            };

            var result = sut.CalculateTotalPrice();

            result.Should().Be(900);
        }

        [Fact]
        public void CalculateTotalWithTax_ValidItems_ReturnsExactTotal()
        {
            sut.Items = new List<OrderItem>()
            {
                new OrderItem(){ Name = "Item1", Price = 100, Quantity = 5},
                new OrderItem(){ Name = "Item2", Price = 200, Quantity = 2},
            };

            var result = sut.CalculateTotalWithTax();

            result.Should().Be(927);
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
            sut.Items = new List<OrderItem>()
            {
                new OrderItem(){ Name = "food1", Quantity = 2, Price = 5},
            };

            bool result = sut.IsEmpty();

            result.Should().BeFalse();
        }
    }
}

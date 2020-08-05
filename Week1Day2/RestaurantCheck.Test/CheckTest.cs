using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RestaurantCheck.Test
{
    
    public class CheckTest
    {
        Check sut;
        public CheckTest()
        {
            sut = new Check();
        }

        [Fact]
        public void CalculateDiscount_TotalGreaterThan20_Returns5PercentOfDiscount()
        {
            var result = sut.CalculateDiscount(30);
            var expected = 1.5;
            result.Should().Be(expected);
        }

        [Fact]
        public void CalculateDiscount_TotalLessThan20_ReturnsZero()
        {
            var result = sut.CalculateDiscount(19);

            result.Should().Be(0);
        }

        [Fact]
        public void CalculateDiscount_TotalEqual20_ReturnsZero()
        {
            var result = sut.CalculateDiscount(20);

            result.Should().Be(0);
        }

        [Fact]
        public void CalculateToTalBeforeTax_WithNoDiscount_ReturnsExactValue()
        {
            sut.Items = new List<CheckItem>()
            {
                new CheckItem(){ Name = "food1", Price = 5},
                new CheckItem(){ Name = "food2", Price = 5}
            };

            var result = sut.CalculateTotalBeforeTax();

            result.Should().Be(10);
        }
        
        [Fact]
        public void CalculateToTalBeforeTax_WithDiscount_ReturnsExactValue2()
        {
            sut.Items = new List<CheckItem>()
            {
                new CheckItem(){ Name = "food1", Price = 17.0},
                new CheckItem(){ Name = "food2", Price = 13.0}
            };
            var result = sut.CalculateTotalBeforeTax();

            result.Should().Be(28.5);
        }

        [Fact]
        public void CalculateTotalAfterTax_WithDiscount_ReturnsExactValue()
        {
            sut.Items = new List<CheckItem>()
            {
                new CheckItem(){ Name = "food1", Price = 50},
                new CheckItem(){ Name = "food2", Price = 50}
            };

            var result = sut.CalculateTotalAfterTax();

            result.Should().Be(96.9);
        }

        [Fact]
        public void CalculateTotalAfterTax_WithNoDiscount_ReturnsExactValue()
        {
            sut.Items = new List<CheckItem>()
            {
                new CheckItem(){ Name = "food1", Price = 5},
                new CheckItem(){ Name = "food2", Price = 5}
            };

            var result = sut.CalculateTotalAfterTax();

            result.Should().Be(10.2);
        }

        [Fact]
        public void IsEmpty_EmptyCheckItems_ReturnsTrue()
        {
            bool result = sut.IsEmpty();

            result.Should().BeTrue();
        }

        [Fact]
        public void IsEmpty_NotEmptyCheckItems_ReturnsTrue()
        {
            sut.Items = new List<CheckItem>()
            {
                new CheckItem(){ Name = "food1", Price = 5},
            };

            bool result = sut.IsEmpty();

            result.Should().BeFalse();
        }

    }
}

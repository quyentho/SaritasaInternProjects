using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
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

        // TODO: Create Test for Calculate().
        [Fact]
        public void Calculate_WithDiscount_ReturnsTotalExactly()
        {
            string input = "food1,50";
            sut.Items = new List<CheckItem>()
            {
                new CheckItem(input),
                new CheckItem(input)
            };

            var checkResult = sut.Calculate();

            checkResult.Should().Match<CheckResult>(r =>
            r.DiscountAmount == 5 &&
            r.TotalBeforeTax == 95 &&
            r.TotalAfterTax == 96.9
            );
        }

        [Fact]
        public void Calculate_WithNoDiscount_ReturnsTotalExactly()
        {
            string input = "food1,5";
            sut.Items = new List<CheckItem>()
            {
                new CheckItem(input),
                new CheckItem(input)
            };

            var checkResult = sut.Calculate();

            checkResult.Should().Match<CheckResult>(r =>
            r.DiscountAmount == 0 &&
            r.TotalBeforeTax == 10 &&
            r.TotalAfterTax == 10.2
            );
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
            string input = "food1,5";
            sut.Items = new List<CheckItem>()
            {
                new CheckItem(input),
            };

            bool result = sut.IsEmpty();

            result.Should().BeFalse();
        }
    }
}

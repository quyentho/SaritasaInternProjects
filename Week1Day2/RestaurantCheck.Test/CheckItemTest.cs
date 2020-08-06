using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RestaurantCheck.Test
{
    public class CheckItemTest
    {
        [Fact]
        public void CheckItem_NegativePrice_ThrowsArgumentOutOfRangeException()
        {
            string input = "food,-1";
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new CheckItem(input));
            Assert.Equal("Price cannot be negative", ex.Message);
        }

        [Fact]
        public void CheckItem_PriceIsNotNumber_ThrowsFormatException()
        {
            string input = "food,food";
            var ex = Assert.Throws<FormatException>(() => new CheckItem(input));
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xunit;

//namespace MarketDesk.Test
//{
//    public class OrderItemTest
//    {
//        [Theory]
//        [InlineData("food1,-1,10")]
//        [InlineData("food1,10,-1")]
//        [InlineData("food1,-1,-10")]
//        public void OrderItem_NegativeQuantityOrPrice_ThrowsArgumentOutOfRangeException(string input)
//        {
//            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem(input));
//            Assert.Equal("Price and Quantity cannot be negative", ex.Message);
//        }

//        [Theory]
//        [InlineData("food1,notNum,10")]
//        [InlineData("food1,10,notNum")]
//        [InlineData("food1,notNum,notNum")]
//        public void OrderItem_PriceAndQuantityIsNotNumber_ThrowsFormatException(string input)
//        {
//            var ex = Assert.Throws<FormatException>(() => new OrderItem(input));
//        }
//    }
//}

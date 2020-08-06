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

        // TODO: Create Test for Calculate().


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

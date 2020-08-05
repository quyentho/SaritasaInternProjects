using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantCheck
{
    /// <summary>
    /// Result to display.
    /// </summary>
    public class CheckResult
    {
        public double TotalBeforeTax { get; set; }

        public double TotalAfterTax { get; set; }

        public double DiscountAmount { get; set; }
    }
}

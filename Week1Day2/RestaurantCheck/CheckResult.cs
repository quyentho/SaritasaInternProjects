// Copyright (c) Saritasa, LLC

namespace RestaurantCheck
{
    /// <summary>
    /// Result to display.
    /// </summary>
    public class CheckResult
    {
        /// <summary>
        /// Gets or sets total price before tax.
        /// </summary>
        public double TotalBeforeTax { get; set; }

        /// <summary>
        /// Gets or sets total price after tax.
        /// </summary>
        public double TotalAfterTax { get; set; }

        /// <summary>
        /// Gets or sets discount amount.
        /// </summary>
        public double DiscountAmount { get; set; }
    }
}

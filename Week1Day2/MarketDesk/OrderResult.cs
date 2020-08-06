// Copyright (c) Saritasa, LLC

namespace MarketDesk
{
    /// <summary>
    /// Result after calculate to display.
    /// </summary>
    public class OrderResult
    {
        /// <summary>
        /// Gets or sets total price before tax.
        /// </summary>
        public double Total { get; set; }

        /// <summary>
        /// Gets or sets total price after tax.
        /// </summary>
        public double TotalWithTax { get; set; }
    }
}
// Copyright (c) Saritasa, LLC

namespace RestaurantCheck
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Contains logic to check totals.
    /// </summary>
    public class Check
    {
        private const double DiscountRate = 0.05;
        private const double TaxRate = 0.02;

        /// <summary>
        /// Gets or sets list check items.
        /// </summary>
        public List<CheckItem> Items { get; set; } = new List<CheckItem>();

        /// <summary>
        /// Check if list item is empty.
        /// </summary>
        /// <returns>True if empty.</returns>
        public bool IsEmpty()
        {
            if (this.Items.Count == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculate Check Result.
        /// </summary>
        /// <returns>CheckResult.</returns>
        public CheckResult Calculate()
        {
            var total = this.Items.Sum(i => i.Price);

            var discountAmount = this.GetDiscountAmout(total);

            var totalBeforeTax = total - discountAmount;

            var totalAfterTax = totalBeforeTax + this.GetTaxAmount(totalBeforeTax);

            return new CheckResult() { DiscountAmount = discountAmount, TotalAfterTax = totalAfterTax, TotalBeforeTax = totalBeforeTax };
        }

        /// <summary>
        /// Calculate discount.
        /// </summary>
        /// <param name="total">Total price to determine if discount.</param>
        /// <returns>total * 5% discount.</returns>
        private double GetDiscountAmout(double total)
        {
            double discountAmount = 0;
            if (total > 20)
            {
                discountAmount = total * DiscountRate;
            }

            return Math.Round(discountAmount, 2);
        }

        private double GetTaxAmount(double totalBeforeTax)
        {
            return Math.Round(totalBeforeTax * TaxRate, 2);
        }
    }
}

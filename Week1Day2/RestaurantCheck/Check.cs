// Copyright (c) Saritasa, LLC

namespace RestaurantCheck
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Contains logic to check totals.
    /// </summary>
    public class Check
    {
        /// <summary>
        /// Gets or sets list check items.
        /// </summary>
        public List<CheckItem> Items { get; set; } = new List<CheckItem>();

        /// <summary>
        /// Calculate total with tax.
        /// </summary>
        /// <returns>Total price after tax.</returns>
        public double CalculateTotalAfterTax()
        {
            double total = this.CalculateTotalBeforeTax();

            double tax = 0.02;

            return Math.Round(total + (total * tax), 2);
        }

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
        /// Calculate total price before tax.
        /// </summary>
        /// <returns>Total price before tax.</returns>
        public double CalculateTotalBeforeTax()
        {
            var total = this.CalculateToTalPriceOfItems();

            return Math.Round(total - this.CalculateDiscount(total), 2);
        }

        private double CalculateToTalPriceOfItems()
        {
            return this.Items.Select(i => i.Price).Sum();
        }

        /// <summary>
        /// Calculate discount.
        /// </summary>
        /// <param name="total">Total price to determine if discount.</param>
        /// <returns>total * 5% discount.</returns>
        public double CalculateDiscount(double total)
        {
            double discount = 0;
            if (total > 20)
            {
                discount = Math.Round(total * 0.05, 2);
            }

            return discount;
        }
     }
}

// Copyright (c) Saritasa, LLC

namespace RestaurantCheck
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <sumary>
    ///  Contains logic to check totals.
    /// </sumary>
    public class Check
    {
        public List<CheckItem> Items { get; set; } = new List<CheckItem>();

        /// <sumary>
        /// Calculate total with tax.
        /// </sumary>
        public double CalculateTotalAfterTax()
        {
            double total = this.CalculateTotalBeforeTax();

            double tax = 0.02;

            return Math.Round(total + (total * tax), 2);
        }

        /// <sumary>
        /// Check if list item is empty.
        /// </sumary>
        public bool IsEmpty()
        {
            if (this.Items.Count == 0)
            {
                return true;
            }

            return false;
        }

        /// <sumary>
        /// Calculate total price before tax.
        /// </sumary>
        public double CalculateTotalBeforeTax()
        {
            var total = this.CalculateToTalPriceOfItems();

            return Math.Round(total - this.CalculateDiscount(total), 2);
        }

        private double CalculateToTalPriceOfItems()
        {
            return this.Items.Select(i => i.Price).Sum();
        }

        /// <sumary>
        /// Calculate discount.
        /// </sumary>
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

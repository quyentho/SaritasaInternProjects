// Copyright (c) Saritasa, LLC

namespace MarketDesk
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Logic to calculate price.
    /// </summary>
    public class Order
    {
        private const double TaxRate = 0.03;

        /// <summary>
        /// Gets or sets list of order items.
        /// </summary>
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        /// <summary>
        /// Calculate total and total with tax.
        /// </summary>
        /// <returns>object of type OrderResult.</returns>
        public OrderResult Calculate()
        {
            var total = this.Items.Sum(i => i.Price * i.Quantity);

            double tax = total * TaxRate;

            double totalWithTax = total + tax;

            return new OrderResult() { Total = total, TotalWithTax = totalWithTax };
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
    }
}

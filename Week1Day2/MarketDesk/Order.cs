// Copyright (c) Saritasa, LLC

namespace MarketDesk
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Logic to calculate price.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Gets or sets list of order items.
        /// </summary>
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        /// <summary>
        /// Calculate total price before tax.
        /// </summary>
        /// <returns>Total price in list item.</returns>
        public double CalculateTotalPrice()
        {
            return this.Items.Select(i => i.Price * i.Quantity).Sum();
        }

        /// <summary>
        /// Calculate total price include tax.
        /// </summary>
        /// <returns>Total price include tax.</returns>
        public double CalculateTotalWithTax()
        {
            double total = this.CalculateTotalPrice();

            double tax = total * 0.03;

            return total + tax;
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

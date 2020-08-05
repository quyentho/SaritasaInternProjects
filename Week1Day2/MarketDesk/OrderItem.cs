// Copyright (c) Saritasa, LLC

namespace MarketDesk
{
    /// <summary>
    /// Represent order item.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Gets or sets name of item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets quantity.
        /// </summary>
        public int Quantity { get; set; }
    }
}
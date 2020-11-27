// Copyright (c) Saritasa, LLC

namespace MarketDesk
{
    using System;

    /// <summary>
    /// Represent order item.
    /// </summary>
    public class OrderItem : IOrderItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem"/> class.
        /// Gets value from input to initialize OrderItem.
        /// </summary>
        /// <param name="input">User input.</param>
        public OrderItem()
        {
        }

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

        /// <summary>
        /// Gets or sets message to display.
        /// </summary>
        public string Message { get; set; }
    }
}
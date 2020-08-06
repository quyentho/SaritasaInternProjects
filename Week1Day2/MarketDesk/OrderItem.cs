// Copyright (c) Saritasa, LLC



namespace MarketDesk
{
    using System;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem"/> class.
        /// Gets value from input to initialize OrderItem.
        /// </summary>
        /// <param name="input">U</param>
        public OrderItem(string input)
        {
            string[] values = input.Split(",");

            this.Name = values[0].Trim();
            this.Quantity = Convert.ToInt32(values[1].Trim());
            this.Price = Convert.ToInt32(values[2].Trim());

            if (this.IsNegative(this.Quantity, this.Price))
            {
                throw new ArgumentOutOfRangeException(string.Empty, "Price and Quantity cannot be negative");
            }
        }

        private bool IsNegative(int quantity, double price)
        {
            return price < 0 || quantity < 0;
        }
    }
}
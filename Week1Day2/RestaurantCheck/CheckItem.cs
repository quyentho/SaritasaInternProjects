// Copyright (c) Saritasa, LLC

namespace RestaurantCheck
{
    using System;

    /// <summary>
    /// Represent Item To Check.
    /// </summary>
    public class CheckItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckItem"/> class. Throws ArgumentOutOfRangeException if price is negative.
        /// </summary>
        /// <param name="input">User input.</param>
        public CheckItem(string input)
        {
            string[] values = input.Split(",");

            this.Name = values[0].Trim();
            this.Price = Convert.ToDouble(values[1].Trim());

            if (this.Price < 0)
            {
                throw new ArgumentOutOfRangeException(string.Empty, "Price cannot be negative");
            }
        }

        /// <summary>
        /// Gets or sets item name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckItem"/> class.
        /// Throw ArgumentOutOfRangeException if price is negative.
        /// </summary>
        /// <param name="input">user input.</param>
    }
}
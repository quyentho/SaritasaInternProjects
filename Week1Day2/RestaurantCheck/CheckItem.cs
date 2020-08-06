// Copyright (c) Saritasa, LLC

namespace RestaurantCheck
{
    using System;

    /// <summary>
    /// Represent Item To Check.
    /// </summary>
    public class CheckItem : ICheckItem
    {
        /// <summary>
        /// Gets or sets item name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets price.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets Message.
        /// </summary>
        public string Message { get; set; }
    }
}
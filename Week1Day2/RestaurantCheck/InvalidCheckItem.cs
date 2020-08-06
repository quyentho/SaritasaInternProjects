// Copyright (c) Saritasa, LLC

namespace RestaurantCheck
{
    /// <summary>
    /// Represent invalid input.
    /// </summary>
    public class InvalidCheckItem : ICheckItem
    {
        /// <summary>
        /// Gets or sets message to display.
        /// </summary>
        public string Message { get; set; }
    }
}

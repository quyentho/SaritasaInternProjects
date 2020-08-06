// Copyright (c) Saritasa, LLC

namespace MarketDesk
{
    /// <summary>
    /// Represent invalid input.
    /// </summary>
    public class InvalidOrderItem : IOrderItem
    {
        /// <summary>
        /// Gets or sets message to display.
        /// </summary>
        public string Message { get; set; }
    }
}

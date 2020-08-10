// Copyright (c) Saritasa, LLC

namespace MarketDesk
{
    using System;
    using System.Linq;

    /// <summary>
    /// Factory class to create Order Item.
    /// </summary>
    public static class OrderItemFactory
    {
        /// <summary>
        /// Contains logic to create order item.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>Proper Order Item.</returns>
        public static IOrderItem Create(string input)
        {
            IOrderItem orderItem = null;

            if (input.Equals("x", StringComparison.InvariantCultureIgnoreCase))
            {
                orderItem = new ExitOrderItem();
                orderItem.Message = "Exit";
                return orderItem;
            }

            if (input.Equals(string.Empty))
            {
                orderItem = new EmptyOrderItem();
                return orderItem;
            }

            string[] values = input.Split(",");
            if (HasInvalidInput(values))
            {
                orderItem = new InvalidOrderItem();
                orderItem.Message = "Invalid input format.";
                return orderItem;
            }

            try
            {
                var quantity = Convert.ToInt32(values[1]);
                var price = Convert.ToDouble(values[2]);
                if (IsNegative(quantity, price))
                {
                    orderItem = new InvalidOrderItem();
                    orderItem.Message = "Price and Quantity cannot negative";
                    return orderItem;
                }

                orderItem = new OrderItem();
                (orderItem as OrderItem).Name = values.First();
                (orderItem as OrderItem).Quantity = quantity;
                (orderItem as OrderItem).Price = price;
            }
            catch (FormatException)
            {
                orderItem = new InvalidOrderItem();
                orderItem.Message = "Price And Quantity must be number";
            }

            return orderItem;
        }

        private static bool HasInvalidInput(string[] values)
        {
            return values.Count() != 3;
        }

        private static bool IsNegative(int quantity, double price)
        {
            return quantity < 0 || price < 0;
        }
    }
}

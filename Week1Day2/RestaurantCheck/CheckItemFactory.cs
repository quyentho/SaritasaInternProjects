// Copyright (c) Saritasa, LLC

namespace RestaurantCheck
{
    using System;
    using System.Linq;

    /// <summary>
    /// Factory class to create Check Item.
    /// </summary>
    public static class CheckItemFactory
    {
        /// <summary>
        /// Contains logic to create Check item.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>Proper Check Item.</returns>
        public static ICheckItem Create(string input)
        {
            ICheckItem checkItem = null;

            if (input.Equals("x", StringComparison.InvariantCultureIgnoreCase))
            {
                checkItem = new ExitCheckItem();
                checkItem.Message = "Exit";
                return checkItem;
            }

            if (input.Equals(string.Empty))
            {
                checkItem = new EmptyCheckItem();
                return checkItem;
            }

            string[] values = input.Split(",");
            if (HasInvalidInput(values))
            {
                checkItem = new InvalidCheckItem();
                checkItem.Message = "Invalid input format.";
                return checkItem;
            }

            try
            {
                var price = Convert.ToDouble(values[1]);
                if (IsNegative(price))
                {
                    checkItem = new InvalidCheckItem();
                    checkItem.Message = "Price cannot negative";
                    return checkItem;
                }

                checkItem = new CheckItem();
                (checkItem as CheckItem).Name = values.First();
                (checkItem as CheckItem).Price = price;
            }
            catch (FormatException)
            {
                checkItem = new InvalidCheckItem();
                checkItem.Message = "Price must be number";
            }

            return checkItem;
        }

        private static bool HasInvalidInput(string[] values)
        {
            return values.Count() != 2;
        }

        private static bool IsNegative(double price)
        {
            return price < 0;
        }
    }
}

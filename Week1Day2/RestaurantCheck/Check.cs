// Copyright (c) Saritasa, LLC

namespace RestaurantCheck
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Contains logic to check totals.
    /// </summary>
    public class Check
    {
        private const double DiscountRate = 0.05;
        private const double TaxRate = 0.02;

        /// <summary>
        /// Gets or sets list check items.
        /// </summary>
        public List<CheckItem> Items { get; set; } = new List<CheckItem>();

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

        /// <summary>
        /// Calculate discount.
        /// </summary>
        /// <param name="total">Total price to determine if discount.</param>
        /// <returns>total * 5% discount.</returns>
        private double GetDiscountAmout(double total)
        {
            double discount = 0;
            if (total > 20)
            {
                discount = total * DiscountRate;
            }

            return discount;
        }

        /// <summary>
        /// Extract food name and price from user input, throw ArgumentOutOfRangeException if price is negative.
        /// </summary>
        /// <param name="input">User input.</param>
        /// <returns>Tuple type string and double represent food name and price.</returns>
        public (string name, double price) GetCheckItemFromInput(string[] input)
        {
            string name = input.First().Trim();
            double price = Convert.ToDouble(input.Last().Trim());
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException("price", "Price Cannot Be Negative");
            }

            return (name, price);
        }

        /// <summary>
        /// Add new item to check item list.
        /// </summary>
        /// <param name="name">food name.</param>
        /// <param name="price">food price.</param>
        public void AddItemToCheckList(string name, double price)
        {
            this.Items.Add(new CheckItem() { Name = name, Price = price });
        }

        /// <summary>
        /// Calculate Check Result.
        /// </summary>
        /// <returns>CheckResult</returns>
        public CheckResult Calculate()
        {
            var total = this.Items.Sum(i => i.Price);

            var discountAmount = this.GetDiscountAmout(total);

            var totalBeforeTax = total - discountAmount;

            var totalAfterTax = totalBeforeTax + GetTaxAmount(totalBeforeTax);

            return new CheckResult() { DiscountAmount = discountAmount, TotalAfterTax = totalAfterTax, TotalBeforeTax = totalBeforeTax };
        }

        private static double GetTaxAmount(double totalBeforeTax)
        {
            return totalBeforeTax * TaxRate;
        }
    }
}

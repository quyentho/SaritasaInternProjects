// Copyright (c) Saritasa, LLC

namespace RestaurantCheck
{
    using System;
    using System.Linq;

    /// <summary>
    /// Application entry point.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Check checkRestaurant = new Check();

            Console.WriteLine("Enter foods you want to check, enter 'x' to quit\n");
            do
            {
                string[] input = Console.ReadLine().Split(',');

                if (InputExitCode(input))
                {
                    break;
                }

                if (InputEmpty(input))
                {
                    if (checkRestaurant.IsEmpty())
                    {
                        Console.WriteLine("Not have item to check, please input:");
                        continue;
                    }

                    double total, discount, totalWithTax;
                    CalculatetTotalPrice(checkRestaurant, out total, out discount, out totalWithTax);
                    DisplayResult(total, discount, totalWithTax);
                }
                else
                {
                    GetCheckItemFromInput(input, checkRestaurant);
                }
            }
            while (true);
        }

        private static void GetCheckItemFromInput(string[] input, Check checkRestaurant)
        {
            try
            {
                string name = input.First().Trim();
                double price = Convert.ToDouble(input.Last().Trim());
                if (price < 0)
                {
                    Console.WriteLine("Price cannot be negative");
                }

                AddToCheckList(checkRestaurant, name, price);
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("input must contain price after comma");
            }
        }

        private static void AddToCheckList(Check checkRestaurant, string name, double price)
        {
            checkRestaurant.Items.Add(new CheckItem { Name = name, Price = price });
        }

        private static bool InputEmpty(string[] input)
        {
            return string.IsNullOrEmpty(input.First());
        }

        private static void CalculatetTotalPrice(Check checkRestaurant, out double total, out double discount, out double totalWithTax)
        {
            total = checkRestaurant.CalculateTotalBeforeTax();
            discount = checkRestaurant.CalculateDiscount(total);
            totalWithTax = checkRestaurant.CalculateTotalAfterTax();
        }

        private static bool InputExitCode(string[] input)
        {
            return input.First() == "x";
        }

        private static void DisplayResult(double total, double discount, double totalWithTax)
        {
            Console.WriteLine($"Discount: {discount}");
            Console.WriteLine($"Total: {total}");
            Console.WriteLine($"total with tax: {totalWithTax}");
        }
    }
}

﻿// Copyright (c) Saritasa, LLC

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

                    CheckResult checkResult = checkRestaurant.Calculate();
                    DisplayResult(checkResult);
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

        private static bool InputExitCode(string[] input)
        {
            return input.First() == "x";
        }

        private static void DisplayResult(CheckResult result)
        {
            Console.WriteLine($"Discount: ${result.DiscountAmount}");
            Console.WriteLine($"Total: ${result.TotalBeforeTax}");
            Console.WriteLine($"total with tax: ${result.TotalAfterTax}");
        }
    }
}

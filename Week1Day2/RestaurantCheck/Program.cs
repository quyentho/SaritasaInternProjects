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
                string input = Console.ReadLine();

                if (InputExitCode(input))
                {
                    break;
                }

                if (InputEmpty(input))
                { // Start Calculate.
                    if (checkRestaurant.IsEmpty())
                    {
                        Console.WriteLine("Not have item to check, please input:");
                        continue;
                    }

                    CheckResult checkResult = checkRestaurant.Calculate();
                    DisplayResult(checkResult);
                }
                else
                { // Get value from input and add to check list.
                    try
                    {
                        CheckItem item = new CheckItem(input);
                        checkRestaurant.Items.Add(item);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Input Price must be a number");
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            while (true);
        }

        private static bool InputEmpty(string input)
        {
            return string.IsNullOrEmpty(input);
        }

        private static bool InputExitCode(string input)
        {
            return input == "x";
        }

        private static void DisplayResult(CheckResult result)
        {
            Console.WriteLine($"Discount: ${result.DiscountAmount}");
            Console.WriteLine($"Total: ${result.TotalBeforeTax}");
            Console.WriteLine($"total with tax: ${result.TotalAfterTax}");
        }
    }
}

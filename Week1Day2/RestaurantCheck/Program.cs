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
            Check check = new Check();

            Console.WriteLine("Enter foods you want to check, enter 'x' to quit\n");
            do
            {
                string input = Console.ReadLine();
                ICheckItem item = CheckItemFactory.Create(input);
                if (item is ExitCheckItem)
                {
                    break;
                }
                else if (item is EmptyCheckItem)
                { // Start Calculate.
                    if (check.IsEmpty())
                    {
                        Console.WriteLine("Not have item to check, please input:");
                        continue;
                    }

                    CheckResult checkResult = check.Calculate();
                    DisplayResult(checkResult);
                }
                else if (item is InvalidCheckItem)
                {
                    Console.WriteLine(item.Message);
                    continue;
                }
                else if (item is CheckItem)
                {
                    check.Items.Add(item as CheckItem);
                }
            }
            while (true);
        }

        private static void DisplayResult(CheckResult result)
        {
            Console.WriteLine($"Discount: ${result.DiscountAmount}");
            Console.WriteLine($"Total: ${result.TotalBeforeTax}");
            Console.WriteLine($"Total with tax: ${result.TotalAfterTax}");
        }
    }
}

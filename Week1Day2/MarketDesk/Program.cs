// Copyright (c) Saritasa, LLC
namespace MarketDesk
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
            Order order = new Order();

            Console.WriteLine("Enter foods you want to Order, enter 'x' to quit\n");
            do
            {
                string input = Console.ReadLine();

                if (InputExitCharacter(input))
                {
                    break;
                }

                if (InputEmpty(input))
                {
                    if (order.IsEmpty())
                    {
                        Console.WriteLine("Not have value to check Order");
                        continue;
                    }

                    OrderResult result = order.Calculate();
                    DisplayResult(result.Total, result.TotalWithTax);
                }
                else
                {
                    try
                    {
                        OrderItem item = new OrderItem(input);
                        order.Items.Add(item);
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Price and Quantity must be number.");
                    }
                }
            }
            while (true);
        }

        private static bool InputEmpty(string input)
        {
            return string.IsNullOrEmpty(input);
        }

        private static bool InputExitCharacter(string input)
        {
            return input == "x";
        }

        private static void DisplayResult(double total, double totalWithTax)
        {
            Console.WriteLine($"Total: ${total}");
            Console.WriteLine($"Total with tax: ${totalWithTax}");
        }
    }
}

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
                string[] input = Console.ReadLine().Split(',');

                if (input.First() == "x")
                {
                    break;
                }

                if (string.IsNullOrEmpty(input.First()))
                {
                    if (order.IsEmpty())
                    {
                        Console.WriteLine("Not have value to Order");
                        continue;
                    }

                    double total = order.CalculateTotalPrice();
                    double totalWithTax = order.CalculateTotalWithTax();

                    DisplayResult(total, totalWithTax);
                }
                else
                {
                    try
                    {
                        string name = input[0].Trim();
                        int quantity = Convert.ToInt32(input[1].Trim());
                        double price = Convert.ToDouble(input[2].Trim());
                        if (price < 0 || quantity < 0)
                        {
                            Console.WriteLine("Price and Quantity cannot be negative");
                            break;
                        }

                        order.Items.Add(new OrderItem { Name = name, Price = price, Quantity = quantity });
                }
                    catch (Exception)
                    {
                        Console.WriteLine("input must contain quantity and price after comma");
                        continue;
                    }
                }
            }
            while (true);
        }

        private static void DisplayResult(double total, double totalWithTax)
        {
            Console.WriteLine($"Total: ${total}");
            Console.WriteLine($"Total with tax: ${totalWithTax}");
        }
    }
}

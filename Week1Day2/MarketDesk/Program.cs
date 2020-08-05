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

                    double total;
                    double totalWithTax;
                    CalculateTotal(order, out total, out totalWithTax);
                    DisplayResult(total, totalWithTax);
                }
                else
                {
                    GetOrderItemFromInput(input, order);
                }
            }
            while (true);
        }

        private static bool InputEmpty(string[] input)
        {
            return string.IsNullOrEmpty(input.First());
        }

        private static bool InputExitCharacter(string[] input)
        {
            return input.First() == "x";
        }

        private static void GetOrderItemFromInput(string[] input, Order order)
        {
            try
            {
                string name = input[0].Trim();
                int quantity = Convert.ToInt32(input[1].Trim());
                double price = Convert.ToDouble(input[2].Trim());
                if (IsNegative(quantity, price))
                {
                    Console.WriteLine("Price and Quantity cannot be negative");
                }

                AddNewOrderItem(order, name, quantity, price);
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Price and Quantity must be decimal");
            }
        }

        private static void AddNewOrderItem(Order order, string name, int quantity, double price)
        {
            order.Items.Add(new OrderItem { Name = name, Price = price, Quantity = quantity });
        }

        private static bool IsNegative(int quantity, double price)
        {
            return price < 0 || quantity < 0;
        }

        private static void CalculateTotal(Order order, out double total, out double totalWithTax)
        {
            total = order.CalculateTotalPrice();
            totalWithTax = order.CalculateTotalWithTax();
        }

        private static void DisplayResult(double total, double totalWithTax)
        {
            Console.WriteLine($"Total: ${total}");
            Console.WriteLine($"Total with tax: ${totalWithTax}");
        }
    }
}

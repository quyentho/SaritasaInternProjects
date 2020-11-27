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

                IOrderItem orderItem = OrderItemFactory.Create(input);

                if (orderItem is ExitOrderItem)
                {
                    break;
                }
                else if (orderItem is EmptyOrderItem)
                {
                    if (order.IsEmpty())
                    {
                        Console.WriteLine("Not have value to check Order");
                        continue;
                    }

                    OrderResult result = order.Calculate();
                    DisplayResult(result.Total, result.TotalWithTax);
                }
                else if (orderItem is InvalidOrderItem)
                {
                    Console.WriteLine(orderItem.Message);
                    continue;
                }
                else if (orderItem is OrderItem)
                {
                        order.Items.Add(orderItem as OrderItem);
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

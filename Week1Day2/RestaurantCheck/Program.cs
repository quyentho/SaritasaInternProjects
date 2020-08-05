namespace RestaurantCheck
{
    using System;
    using System.Linq;
    class Program
    {
        static void Main(string[] args)
        {
            Check checkRestaurant = new Check();

            Console.WriteLine("Enter foods you want to check, enter 'x' to quit\n");
            do
            {

                string[] input = Console.ReadLine().Split(',');

                if (input.First() == "x")
                {
                    break;
                }

                if (string.IsNullOrEmpty(input.First()))
                {
                    if (checkRestaurant.IsEmpty())
                    {
                        Console.WriteLine("Not have value to check");
                        continue;
                    }

                    var total = checkRestaurant.CalculateTotalBeforeTax();
                    var discount = checkRestaurant.CalculateDiscount(total);
                    var totalWithTax = checkRestaurant.CalculateTotalAfterTax();

                    DisplayResult(total, discount, totalWithTax);
                }
                else
                {
                    try
                    {
                        string name = input.First().Trim();
                        double price = Convert.ToDouble(input.Last().Trim());
                        if (price < 0)
                        {
                            Console.WriteLine("Price cannot be negative");
                            break;
                        }

                        checkRestaurant.Items.Add(new CheckItem { Name = name, Price = price });
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("input must contain price after comma");
                        continue;
                    }
                }
            } while (true);
        }

        private static void DisplayResult(double total, double discount, double totalWithTax)
        {
            Console.WriteLine($"Discount: {discount}");
            Console.WriteLine($"Total: {total}");
            Console.WriteLine($"total with tax: {totalWithTax}");
        }
    }
}

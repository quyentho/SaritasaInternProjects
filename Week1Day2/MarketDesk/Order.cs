// Copyright (c) Saritasa, LLC

namespace MarketDesk
{
    using System.Collections.Generic;
    using System.Linq;

    // <sumary>
    // Order logic to calculate price.
    // </sumary>
    public class Order
    {
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public double CalculateTotalPrice()
        {
            return this.Items.Select(i => i.Price * i.Quantity).Sum();
        }

        public double CalculateTotalWithTax()
        {
            double total = this.CalculateTotalPrice();

            double tax = total * 0.03;

            return total + tax;
        }

        public bool IsEmpty()
        {
            if (this.Items.Count == 0)
            {
                return true;
            }

            return false;
        }
    }
}

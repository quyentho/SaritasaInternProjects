using System;

namespace CurrencyReader.Model
{
    /// <summary>
    /// Represent currency data in file.
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Gets or sets Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets Ruble rate.
        /// </summary>
        public double Ruble { get; set; }

        /// <summary>
        /// Gets or sets Euro rate.
        /// </summary>
        public double Euro { get; set; }
    }
}

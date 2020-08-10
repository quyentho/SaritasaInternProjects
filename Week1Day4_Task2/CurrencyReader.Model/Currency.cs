using System;

namespace CurrencyReader.Model
{
    /// <summary>
    /// Represent currency data in file.
    /// </summary>
    public class Currency
    {
        // TODO: Format Datetime to display properly: yyyy-mm-dd

        /// <summary>
        /// Gets or sets Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets Ruble rate.
        /// </summary>
        public double Rub { get; set; }

        /// <summary>
        /// Gets or sets Euro rate.
        /// </summary>
        public double Eur { get; set; }
    }
}

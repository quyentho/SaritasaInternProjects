using CurrencyReader.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CurrencyReader.Service
{
    /// <summary>
    /// Service for find currency.
    /// </summary>
    public class FindCurrencyService : IFindCurrencyService
    {
        /// <summary>
        /// Find currency item.
        /// </summary>
        /// <param name="currencies">List currency read from file.</param>
        /// <param name="input">User input.</param>
        /// <returns>Currency object if found.</returns>
        public Currency GetCurrency(List<Currency> currencies, string input)
        {
            try
            {
                var date = DateTime.ParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Currency result = currencies.Find(c => c.Date == date);
                return result;
            }
            catch (FormatException)
            {
                throw;
            }
        }
    }
}

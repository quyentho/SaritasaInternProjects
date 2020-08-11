namespace CurrencyReader.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using CurrencyReader.Model;

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
        public SearchResult GetCurrencies(List<Currency> currencies, string input)
        {
            try
            {
                IEnumerable<DateTime> dates = this.GetListDateFromInput(input);
                SearchResult result = new SearchResult();
                result.FoundItems = currencies.Where(c => dates.Contains(c.Date)).ToList();
                if (result.FoundItems.Count <= 0)
                {
                    throw new CurrencyNotFoundException($"{input}: [!] Currency for specific date is not found.");
                }

                result.NotFoundDates = this.GetNotFoundDates(dates, result);
                return result;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CurrencyNotFoundException)
            {
                throw;
            }
        }

        private IEnumerable<DateTime> GetNotFoundDates(IEnumerable<DateTime> dates, SearchResult result)
        {
            IEnumerable<DateTime> foundDates = result.FoundItems.Select(c => c.Date);
            return dates.Except(foundDates).ToList();
        }

        private IEnumerable<DateTime> GetListDateFromInput(string input)
        {
            string[] listDate = input.Split(",", StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<DateTime> dates = listDate
                .Select(d => DateTime.ParseExact(d, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            return dates;
        }
    }
}

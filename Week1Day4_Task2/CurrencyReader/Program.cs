namespace CurrencyReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CurrencyReader.Model;
    using CurrencyReader.Service;

    /// <summary>
    /// Program class.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">Application arguments.</param>
        private static void Main(string[] args)
        {
            try
            {
                var readCurrencyservice = new ReadCurrencyService();

                List<Currency> data = readCurrencyservice.ReadFromFile();
                do
                {
                    var input = Console.ReadLine();
                    SearchResult result = FindCurrenciesFromData(data, input);
                    DisplayResult(result);
                }
                while (true);
            }
            catch (FormatException)
            {
                Console.WriteLine("Input must in (yyyy-MM-dd) format.");
            }
            catch (CurrencyNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static SearchResult FindCurrenciesFromData(List<Currency> data, string input)
        {
            IFindCurrencyService findCurrencyService = new FindCurrencyService();
            findCurrencyService = new CacheFindCurrencyService(findCurrencyService);

            SearchResult result = findCurrencyService.GetCurrencies(data, input);

            return result;
        }

        private static void DisplayResult(SearchResult result)
        {
            foreach (var currency in result.FoundItems)
            {
                Console.WriteLine($"{currency.Date.ToShortDateString()} : {currency.Rub} RUB");
            }

            if (result.NotFoundDates.Count() > 0)
            {
                foreach (var date in result.NotFoundDates)
                {
                    Console.WriteLine($"{date}: [!] Currency for specific date is not found.");
                }
            }
        }
    }
}

using CurrencyReader.Model;
using CurrencyReader.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ReadCurrencyService readCurrencyservice = new ReadCurrencyService();

                List<Currency> data = readCurrencyservice.ReadFromFile();
                // TODO: Refactor for more readable.
                do
                {
                    var input = Console.ReadLine();
                    var cachedService = new CachedFindCurrencyReaderService(new FindCurrencyService());
                    SearchResult result = cachedService.GetCurrencies(data, input);
                    foreach (var currency in result.FoundItems)
                    {
                        Console.WriteLine($"{currency.Date} : {currency.Rub} RUB");
                    }

                    if (result.NotFoundDates.Count() > 0)
                    {
                        foreach (var date in result.NotFoundDates)
                        {
                            Console.WriteLine($"{date}: [!] Currency for specific date is not found.");
                        }
                    }
                } while (true);
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
    }
}

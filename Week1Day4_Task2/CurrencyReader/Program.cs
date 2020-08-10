using CurrencyReader.Model;
using CurrencyReader.Service;
using System;
using System.Collections.Generic;

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

                var input = Console.ReadLine();
                var cachedService = new CachedFindCurrencyReaderService(new FindCurrencyService());
                var result = cachedService.GetCurrency(data, input);

                Console.WriteLine($"{result.Date} : {result.Rub} RUB");
            }
            catch (FormatException)
            {
                Console.WriteLine("Input must in (yyyy-MM-dd) format.");
            }
        }
    }
}

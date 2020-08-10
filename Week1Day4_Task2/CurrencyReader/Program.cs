using CurrencyReader.Service;
using System;

namespace CurrencyReader
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadCurrencyService service = new ReadCurrencyService();

            var data = service.ReadFromFile();
        }
    }
}


using CurrencyReader.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace CurrencyReader.Service
{
    /// <summary>
    /// Read data service.
    /// </summary>
    public class ReadCurrencyService : IReadCurrencyService
    {
        public List<Currency> ReadFromFile()
        {
            var file = File.ReadAllText(ConfigurationManager.AppSettings["Path"]);

            List<Currency> currencyItems = JsonConvert.DeserializeObject<List<Currency>>(file);

            return currencyItems;
        }
    }
}


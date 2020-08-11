// Copyright (c) Saritasa, LLC

using CurrencyReader.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Resources;
namespace CurrencyReader.Service
{
    /// <summary>
    /// Read data service.
    /// </summary>
    public class ReadCurrencyService : IReadCurrencyService
    {
        /// <summary>
        /// Read data from json file.
        /// </summary>
        /// <returns>List of currency item.</returns>
        public List<Currency> ReadFromFile()
        {
            var file = File.ReadAllText(ConfigurationManager.AppSettings["Path"]);

            List<Currency> currencyItems = JsonConvert.DeserializeObject<List<Currency>>(file);

            return currencyItems;
        }
    }
}


using CurrencyReader.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace CurrencyReader.Service
{
    public class CurrencyReaderService : ICurrencyReaderService
    {
        public List<Currency> ReadFromFile()
        {
            using (StreamReader file = File.OpenText(ConfigurationManager.AppSettings["Path"]))
            {

                List<Currency> currencies = JsonConvert.DeserializeObject<List<Currency>>(file)
            }
        }
    }
}

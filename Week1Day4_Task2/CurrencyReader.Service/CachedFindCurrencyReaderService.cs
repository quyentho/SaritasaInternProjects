using CurrencyReader.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyReader.Service
{
    public class CachedFindCurrencyReaderService : IFindCurrencyService
    {
        private readonly IFindCurrencyService findCurrencyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedFindCurrencyReaderService"/> class.
        /// </summary>
        /// <param name="findCurrencyService">Instance of IFindCurrency.</param>
        public CachedFindCurrencyReaderService(IFindCurrencyService findCurrencyService)
        {
            this.findCurrencyService = findCurrencyService;
        }

        /// <summary>
        /// Gets or sets Cached list of currency item.
        /// </summary>
        public List<Currency> CachedList { get; set; } = new List<Currency>();

        /// <summary>
        /// Finds currency and cached 10 recent found items.
        /// </summary>
        /// <param name="currencies">List of currency from file.</param>
        /// <param name="input">User input.</param>
        /// <returns>Currency object if found.</returns>
        public Currency GetCurrency(List<Currency> currencies, string input)
        {
            Currency result = this.findCurrencyService.GetCurrency(currencies, input);

            this.CacheResult(result);

            return result;
        }

        private void CacheResult(Currency result)
        {
            if (this.CachedList.Count > 10)
            {
                this.CachedList.RemoveAt(0);
            }

            this.CachedList.Add(result);
        }
    }
}

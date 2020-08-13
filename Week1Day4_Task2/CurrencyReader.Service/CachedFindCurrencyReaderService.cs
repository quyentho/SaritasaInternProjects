namespace CurrencyReader.Service
{
    using System.Collections.Generic;
    using CurrencyReader.Model;

    /// <summary>
    /// Decorator class to cach find result.
    /// </summary>
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
        public SearchResult GetCurrencies(List<Currency> currencies, string input)
        {
            SearchResult result = this.findCurrencyService.GetCurrencies(currencies, input);

            this.CacheResult(result.FoundItems);

            return result;
        }

        private void CacheResult(List<Currency> foundItems)
        {
            int numberOfItemsAfterAdded = this.CachedList.Count + foundItems.Count;
            if (numberOfItemsAfterAdded > 10)
            {
                this.CachedList.RemoveRange(0, numberOfItemsAfterAdded - 10);
            }

            this.CachedList.AddRange(foundItems);
        }
    }
}

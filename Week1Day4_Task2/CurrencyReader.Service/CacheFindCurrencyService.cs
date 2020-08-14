namespace CurrencyReader.Service
{
    using System.Collections.Generic;
    using CurrencyReader.Model;

    /// <summary>
    /// Decorator class to cach find result.
    /// </summary>
    public class CacheFindCurrencyService : BaseFindCurrencyService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheFindCurrencyService"/> class.
        /// </summary>
        /// <param name="findCurrencyService">Instance of IFindCurrency.</param>
        public CacheFindCurrencyService(IFindCurrencyService findCurrencyService)
            : base(findCurrencyService)
        {
        }

        /// <summary>
        /// Gets or sets Cached list of currency item.
        /// </summary>
        public List<Currency> CachedList { get; set; } = new List<Currency>();

        /// <summary>
        /// Caches 10 recent found value.
        /// </summary>
        /// <param name="result">Result to cache.</param>
        public override void PostProcess(SearchResult result)
        {
            this.CacheResult(result.FoundItems);
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

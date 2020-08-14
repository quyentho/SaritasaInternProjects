using CurrencyReader.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyReader.Service
{
    /// <summary>
    /// Decorator for Find currency service.
    /// </summary>
    public abstract class FindCurrencyServiceDecorator : IFindCurrencyService
    {
        private IFindCurrencyService findCurrencyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCurrencyServiceDecorator"/> class.
        /// </summary>
        /// <param name="findCurrencyService">IFindCurrencyService for DI.</param>
        public FindCurrencyServiceDecorator(IFindCurrencyService findCurrencyService)
        {
            this.findCurrencyService = findCurrencyService;
        }

        /// <summary>
        /// Call to base class and decorate more service.
        /// </summary>
        /// <param name="currencies">List currency read from file.</param>
        /// <param name="input">User input.</param>
        /// <returns>Currency object if found.</returns>
        public SearchResult GetCurrencies(List<Currency> currencies, string input)
        {
            var result = this.findCurrencyService.GetCurrencies(currencies, input);

            Decorate(result);

            return result;
        }

        /// <summary>
        /// Additional functionality after find currency.
        /// </summary>
        public abstract void Decorate(SearchResult result);
    }
}

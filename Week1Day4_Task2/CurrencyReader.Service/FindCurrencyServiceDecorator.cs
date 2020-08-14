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

        public SearchResult GetCurrencies(List<Currency> currencies, string input)
        {
            throw new NotImplementedException();
        }
    }
}

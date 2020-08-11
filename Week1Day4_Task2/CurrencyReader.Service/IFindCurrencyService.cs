// Copyright (c) Saritasa, LLC

namespace CurrencyReader.Service
{
    using System.Collections.Generic;
    using CurrencyReader.Model;

    /// <summary>
    /// Interface for find service .
    /// </summary>
    public interface IFindCurrencyService
    {
        /// <summary>
        /// Find currency item.
        /// </summary>
        /// <param name="currencies">List currency read from file.</param>
        /// <param name="input">User input.</param>
        /// <returns>Currency object if found.</returns>
        SearchResult GetCurrencies(List<Currency> currencies, string input);
    }
}

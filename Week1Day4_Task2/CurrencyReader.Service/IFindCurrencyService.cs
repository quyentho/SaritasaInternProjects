// Copyright (c) Saritasa, LLC

using CurrencyReader.Model;
using System.Collections.Generic;

namespace CurrencyReader.Service
{
    /// <summary>
    /// Find service interface.
    /// </summary>
    public interface IFindCurrencyService
    {
        /// <summary>
        /// Find currency item.
        /// </summary>
        /// <param name="currencies">List currency read from file.</param>
        /// <param name="input">User input.</param>
        /// <returns>Currency object if found.</returns>
        Currency GetCurrency(List<Currency> currencies, string input);
    }
}

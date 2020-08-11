namespace CurrencyReader.Service
{
    using System.Collections.Generic;
    using CurrencyReader.Model;

    /// <summary>
    /// Interface for read service.
    /// </summary>
    public interface IReadCurrencyService
    {
        /// <summary>
        /// Read list of currency from json file.
        /// </summary>
        /// <returns>List of currency.</returns>
        List<Currency> ReadFromFile();
    }
}

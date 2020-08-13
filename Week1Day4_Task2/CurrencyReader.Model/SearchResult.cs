namespace CurrencyReader.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents result after searched.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets list of found currency.
        /// </summary>
        public List<Currency> FoundItems { get; set; }

        /// <summary>
        /// Gets or sets list of input dates not found value.
        /// </summary>
        public IEnumerable<DateTime> NotFoundDates { get; set; }
    }
}

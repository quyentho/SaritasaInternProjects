using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyReader.Model
{
    public class SearchResult
    {
        public List<Currency> FoundItems { get; set; }

        public IEnumerable<DateTime> NotFoundDates { get; set; }
    }
}

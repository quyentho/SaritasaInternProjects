using CurrencyReader.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyReader.Service
{
    public interface IReadCurrencyService
    {
        List<Currency> ReadFromFile();
    }
}

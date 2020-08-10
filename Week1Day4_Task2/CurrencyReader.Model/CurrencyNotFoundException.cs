using Saritasa.Tools.Domain.Exceptions;
using System;

namespace CurrencyReader.Model
{
    public class CurrencyNotFoundException : DomainException
    {
        public CurrencyNotFoundException() : base()
        {
        }

        public CurrencyNotFoundException(string message) : base(message)
        {
        }

        public CurrencyNotFoundException(string message, Exception innerException) : base(message,innerException)
        {
        }
    }
}

namespace CurrencyReader.Model
{
    using System;
    using Saritasa.Tools.Domain.Exceptions;

    /// <summary>
    /// Throws when all input not found currency.
    /// </summary>
    public class CurrencyNotFoundException : DomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyNotFoundException"/> class.
        /// </summary>
        public CurrencyNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        public CurrencyNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public CurrencyNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

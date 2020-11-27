// <copyright file="Currency.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CurrencyReader.Model
{
    using System;

    /// <summary>
    /// Represent currency data in file.
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Gets or sets Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets Ruble rate.
        /// </summary>
        public double Rub { get; set; }

        /// <summary>
        /// Gets or sets Euro rate.
        /// </summary>
        public double Eur { get; set; }
    }
}

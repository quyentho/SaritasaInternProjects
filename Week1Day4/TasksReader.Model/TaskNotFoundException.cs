// Copyright (c) Saritasa, LLC

namespace TasksReader.Model
{
    using System;
    using Saritasa.Tools.Domain.Exceptions;

    /// <summary>
    /// Throws when all input ids not found tasks.
    /// </summary>
    public class TaskNotFoundException : DomainException
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotFoundException"/> class.
        /// </summary>
        public TaskNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        public TaskNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotFoundException"/> class.
        /// </summary>
        /// <param name="code">Error code.</param>
        public TaskNotFoundException(int code)
            : base(code)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="code">Error code.</param>
        public TaskNotFoundException(string message, int code)
            : base(message, code)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        public TaskNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="innerException">Inner exception.</param>
        /// <param name="code">Error code.</param>
        public TaskNotFoundException(string message, Exception innerException, int code)
            : base(message, innerException, code)
        {
        }
    }
}

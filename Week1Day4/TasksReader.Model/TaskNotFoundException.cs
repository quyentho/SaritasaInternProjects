using Saritasa.Tools.Domain.Exceptions;
using System;

namespace TasksReader.Model
{
    public class TaskNotFoundException : DomainException
    {
        public TaskNotFoundException() : base()
        {
        }

        public TaskNotFoundException(string message)
            : base(message)
        {
        }

        public TaskNotFoundException(int code)
            : base(code)
        {
        }

        public TaskNotFoundException(string message, int code) : base(message,code)
        {
        }
      
        public TaskNotFoundException(string message, Exception innerException) :base(message,innerException)
        {
        }

        public TaskNotFoundException(string message, Exception innerException, int code) : base(message,innerException,code)
        {
        }
    }
}

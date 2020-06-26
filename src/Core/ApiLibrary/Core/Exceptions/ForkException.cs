using System;

namespace ApiLibrary.Core.Exceptions
{
    public class ForkException : Exception
    {
        public ForkException(string message) : base(message)
        {
        }

    }
}

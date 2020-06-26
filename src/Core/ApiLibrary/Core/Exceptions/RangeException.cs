using System;

namespace ApiLibrary.Core.Exceptions
{
    public class RangeException : Exception
    {
        public RangeException(string message) : base(message)
        {
        }
    }
}

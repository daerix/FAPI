using System;

namespace ApiLibrary.Core.Exceptions
{
    public class SearchException : Exception
    {
        public SearchException(string msg) : base(msg)
        {
        }
    }
}

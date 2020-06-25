using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Core.Exceptions
{
    class SearchException : Exception
    {
        public SearchException(string msg) : base(msg)
        {
        }
    }
}

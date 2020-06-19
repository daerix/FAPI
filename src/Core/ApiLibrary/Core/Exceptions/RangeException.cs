using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Core.Exceptions
{
    public class RangeException: Exception
    {
        public RangeException(string message): base(message)
        {
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Core.Exceptions
{
    class ForkException : Exception
    {
        public ForkException(string message) : base(message)
        {
        }

    }
}

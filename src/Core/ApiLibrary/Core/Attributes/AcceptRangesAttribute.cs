using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    class AcceptRangesAttribute : Attribute
    {
        public int AcceptRange { get; private set; }

        public AcceptRangesAttribute(int value)
        {
            AcceptRange = value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MaxPaginationAttribute : Attribute
    {
        public int Max { get; private set; }

        public MaxPaginationAttribute(int max)
        {
            Max = max;
        }
    }
}

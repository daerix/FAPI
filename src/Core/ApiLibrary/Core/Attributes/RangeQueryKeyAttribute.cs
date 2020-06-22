using System;

namespace ApiLibrary.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    class RangeQueryKeyAttribute : Attribute
    {
        public string RangeQueryKey { get; private set; }

        public RangeQueryKeyAttribute(string value)
        {
            RangeQueryKey = value;
        }
    }
}

using System;

namespace ApiLibrary.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    class SortQueryKeyAttribute : Attribute
    {
        public string SortQueryKey { get; private set; }

        public SortQueryKeyAttribute(string value)
        {
            SortQueryKey = value;
        }

    }
}

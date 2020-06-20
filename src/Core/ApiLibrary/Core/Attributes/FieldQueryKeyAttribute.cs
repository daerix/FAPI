using System;

namespace ApiLibrary.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    class FieldQueryKeyAttribute : Attribute
    {
        public string FieldQueryKey { get; private set; }

        public FieldQueryKeyAttribute(string value)
        {
            FieldQueryKey = value;
        }
    }
}

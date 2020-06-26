using System;

namespace ApiLibrary.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NoModifiedAttribute : Attribute
    {
        public NoModifiedAttribute()
        {
        }
    }
}

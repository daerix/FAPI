using System;
using System.Collections.Generic;
using System.Text;

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

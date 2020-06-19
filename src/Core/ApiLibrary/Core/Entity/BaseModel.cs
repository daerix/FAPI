using ApiLibrary.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Core.Entity
{
    public class BaseModel<T> : BaseEntity
    {
        [NoModified]
        public T ID { get; set; }
    }
}

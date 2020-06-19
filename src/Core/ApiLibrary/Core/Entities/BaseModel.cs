using ApiLibrary.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLibrary.Core.Entities
{
    public class BaseModel<T> : BaseEntity
    {
        [NoModified]
        public T Id { get; set; }
    }
}

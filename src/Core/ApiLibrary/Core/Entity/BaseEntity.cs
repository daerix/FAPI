using ApiLibrary.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLibrary.Core.Entity
{
    public abstract class BaseEntity
    {
        [NoModified]
        public DateTime CreatedAt { get; set; }

        [NoModified]
        public DateTime? UpdatedAt { get; set; }

        [NoModified]
        public DateTime? DeletedAt { get; set; }

    }
}

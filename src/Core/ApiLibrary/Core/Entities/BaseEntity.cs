using ApiLibrary.Core.Attributes;
using System;

namespace ApiLibrary.Core.Entities
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

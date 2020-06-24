using ApiLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models
{
    public enum Type { Male, Female }

    public class Genre : BaseModel<int>
    {
        [Required(ErrorMessage = "Le Type est obligatoire.")]
        public Type Type { get; set; }

    }
}

using ApiLibrary.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models
{
    public class Category : BaseModel<int>
    {
        [StringLength(150)]
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}

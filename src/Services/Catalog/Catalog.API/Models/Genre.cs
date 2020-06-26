using ApiLibrary.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Models
{
    public enum Type { Male, Female }

    public class Genre : BaseModel<int>
    {
        [Required(ErrorMessage = "Le Type est obligatoire.")]
        public Type Type { get; set; }

    }
}

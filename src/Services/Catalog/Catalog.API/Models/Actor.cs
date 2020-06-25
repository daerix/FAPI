using ApiLibrary.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.API.Models
{
    public class Actor : BaseModel<int>
    {

        //[Column("Nom", TypeName = "blob")]
        [StringLength(150)]
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        public string Name { get; set; }

        public float Size { get; set; }
        public float Weight { get; set; }

        public string Description { get; set; }

        public string Attraits { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        [Required]
        public decimal Price { get; set; }


        public int? GenreID { get; set; }
        [ForeignKey("GenreID")]
        public Genre Genre { get; set; }

    }
}

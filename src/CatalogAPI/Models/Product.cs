
using ApiLibrary.Core.Attributes;
using ApiLibrary.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Models
{
    //[Table("Toto", Schema = "catalog")]
  
    public class Product : BaseModel<int>
    {
        //public int ID { get; set; }

        //[Column("Nom", TypeName = "blob")]
        [StringLength(150)]
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        [Required]
        public decimal Price { get; set; }


        public int? GenreID { get; set; }
        [ForeignKey("GenreID")]
        public Genre Genre { get; set; }

    }

    /*public class ProductDTO
    {
        public int ID { get; set; }

        [StringLength(150)]
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        [Required]
        public decimal Price { get; set; }

        public void Map(Product p)
        {
            p.Name = this.Name;
            p.Price = this.Price;
            p.Description = this.Description;
        }
    }*/
}

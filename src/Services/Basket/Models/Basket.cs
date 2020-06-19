using ApiLibrary.Core.Attributes;
using ApiLibrary.Core.Entity;
using Basket.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basket.Models
{
    public class Basket : BaseModel<int>
    {
        [Required]
        [NoModified]
        [Column("id", TypeName = "decimal(6,2)")]
        public int User { get; set; }

        [Column("product_ids")]
        public int[] ProductIDs { get; set; }

        [Required]
        [Column("price", TypeName = "decimal(6,2)")]
        public decimal? Price { get; set; }

        [Required]
        [DefaultValue(BasketStates.PENDING)]
        public BasketStates State { get; set; }
    }
}
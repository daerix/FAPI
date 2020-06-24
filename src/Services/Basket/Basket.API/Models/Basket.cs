using ApiLibrary.Core.Attributes;
using ApiLibrary.Core.Entities;
using Basket.API.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basket.API.Models
{
    public class Basket : BaseModel<int>
    {
        [Required]
        [NoModified]
        [Column("user", TypeName = "decimal(6,2)")]
        public int User { get; set; }

        [Required]
        [DefaultValue(BasketStates.PENDING)]
        public BasketStates State { get; set; }
    }
}
using ApiLibrary.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basket.API.Models
{
    public class Booking : BaseModel<int>
    {

        [Column("product_id")]
        public int ProductID { get; set; }

        [Column("booking_date")]
        public DateTime BookingDate { get; set; }

        [Required]
        [Column("price", TypeName = "decimal(6,2)")]
        public decimal? Price { get; set; }

        public int? BasketID { get; set; }
        [ForeignKey("BasketID")]
        public Basket Basket { get; set; }
    }
}

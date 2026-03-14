using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartnerAccounting.Core.Ivanov.Models
{

    [Table("sales_history", Schema = "app")]
    public class SaleHistory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("partner_id")]
        [ForeignKey("Partner")]
        public int PartnerId { get; set; }

        [Required]
        [Column("product_id")]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [Column("quantity")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Column("sale_date")]
        public DateTime SaleDate { get; set; }

        [Required]
        [Column("total_amount")]
        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual Partner Partner { get; set; }
        public virtual Product Product { get; set; }
    }
}
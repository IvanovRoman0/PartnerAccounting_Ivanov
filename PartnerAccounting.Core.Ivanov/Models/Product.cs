using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartnerAccounting.Core.Ivanov.Models
{

    [Table("products", Schema = "app")]
    public class Product
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("article")]
        [MaxLength(50)]
        public string Article { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("min_partner_price")]
        [Range(0.01, double.MaxValue)]
        public decimal MinPartnerPrice { get; set; }

        public virtual ICollection<SaleHistory> SalesHistory { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PartnerAccounting.Core.Ivanov.Models
{

    [Table("partners", Schema = "app")]
    public class Partner
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("type_id")]
        [ForeignKey("PartnerType")]
        public int TypeId { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("legal_address")]
        [MaxLength(300)]
        public string LegalAddress { get; set; } = string.Empty;

        [Required]
        [Column("inn")]
        [MaxLength(12)]
        public string Inn { get; set; } = string.Empty;

        [Required]
        [Column("director_name")]
        [MaxLength(200)]
        public string DirectorName { get; set; } = string.Empty;

        [Required]
        [Column("phone")]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("rating")]
        [Range(0, int.MaxValue)]
        public int Rating { get; set; }

        [Column("logo_path")]
        [MaxLength(500)]
        public string? LogoPath { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public virtual PartnerType? PartnerType { get; set; }
        public virtual ICollection<SaleHistory> SalesHistory { get; set; } = new List<SaleHistory>();

        [NotMapped]
        public string DiscountDisplay
        {
            get
            {
                decimal totalSales = SalesHistory?.Sum(s => s.TotalAmount) ?? 0;
                int discount = CalculateDiscount(totalSales);
                return $"{discount}%";
            }
        }

        [NotMapped]
        public string TotalSalesDisplay
        {
            get
            {
                decimal total = SalesHistory?.Sum(s => s.TotalAmount) ?? 0;
                return $"{total:N2} руб.";
            }
        }

        public int CalculateDiscount(decimal totalSalesAmount) => totalSalesAmount switch
        {
            < 10000 => 0,
            >= 10000 and < 50000 => 5,
            >= 50000 and < 300000 => 10,
            >= 300000 => 15
        };
    }
}
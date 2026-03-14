using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartnerAccounting.Core.Ivanov.Models
{

    [Table("partner_types", Schema = "app")]
    public class PartnerType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Partner> Partners { get; set; }
    }
}
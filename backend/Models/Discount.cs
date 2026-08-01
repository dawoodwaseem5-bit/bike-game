using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Discount : BaseEntity
{
    [Key]
    public int DiscountId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Code { get; set; }

    [Required]
    [MaxLength(20)]
    public string DiscountType { get; set; } = "Percentage";

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MinOrderAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MaxOrderAmount { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
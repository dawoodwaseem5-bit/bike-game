using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class DiscountEvaluation
{
    [Key]
    public int EvaluationId { get; set; }

    [Required]
    public int QuotationItemId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal ProposedDiscount { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal NormalRangeMin { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal NormalRangeMax { get; set; }

    [Column(TypeName = "decimal(8,4)")]
    public decimal AnomalyScore { get; set; }

    public bool IsAnomaly { get; set; }

    [MaxLength(1000)]
    public string Explanation { get; set; } = string.Empty;

    public bool UserOverride { get; set; }

    public bool? UserConfirmation { get; set; }

    [Required]
    [MaxLength(100)]
    public string EvaluatedBy { get; set; } = string.Empty;

    public DateTime EvaluatedAt { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string CreatedBy { get; set; } = string.Empty;

    [ForeignKey("QuotationItemId")]
    public QuotationItem? QuotationItem { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
}
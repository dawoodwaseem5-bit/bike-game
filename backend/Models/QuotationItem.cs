using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class QuotationItem : BaseEntity
{
    [Key]
    public int QuotationItemId { get; set; }

    [Required]
    public int QuotationId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int Quantity { get; set; }


    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal DiscountPercent { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal TaxRate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal LineTotal { get; set; }

    [ForeignKey("QuotationId")]
    public Quotation? Quotation { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    public ICollection<DiscountEvaluation> DiscountEvaluations { get; set; } = new List<DiscountEvaluation>();
}

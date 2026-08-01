using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Product : BaseEntity
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }


    public int StockQuantity { get; set; } = 0;

    public ICollection<QuotationItem> QuotationItems { get; set; } = new List<QuotationItem>();
    public ICollection<DiscountEvaluation> DiscountEvaluations { get; set; } = new List<DiscountEvaluation>();
}
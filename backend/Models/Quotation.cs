using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Quotation : BaseEntity
{
    [Key]
    public int QuotationId { get; set; }

    [Required]
    [MaxLength(50)]
    public string QuotationNumber { get; set; } = string.Empty;

    [Required]
    public int CustomerId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal TaxRate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public DateTime? ValidUntil { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    public ICollection<QuotationItem> QuotationItems { get; set; } = new List<QuotationItem>();
    public Approval? Approval { get; set; }
    public ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
}

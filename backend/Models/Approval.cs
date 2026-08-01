using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Approval : BaseEntity
{
    [Key]
    public int ApprovalId { get; set; }

    [Required]
    public int QuotationId { get; set; }

    [Required]
    [MaxLength(100)]
    public string RequestedBy { get; set; } = string.Empty;

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [MaxLength(1000)]
    public string? Remarks { get; set; }

    [ForeignKey("QuotationId")]
    public Quotation? Quotation { get; set; }
}
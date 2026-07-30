using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class StatusHistory
{
    [Key]
    public int StatusHistoryId { get; set; }

    [Required]
    public int QuotationId { get; set; }

    [MaxLength(50)]
    public string OldStatus { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string NewStatus { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ChangedBy { get; set; } = string.Empty;

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string CreatedBy { get; set; } = string.Empty;

    [ForeignKey("QuotationId")]
    public Quotation? Quotation { get; set; }
}

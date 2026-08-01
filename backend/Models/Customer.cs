using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Customer : BaseEntity
{
    [Key]
    public int CustomerId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;


    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(200)]
    public string? Company { get; set; }

    public ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}
using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.DTOs
{
    public class QuotationCreateDto
    {
        public int CustomerId { get; set; }
        public decimal TaxRate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public List<QuotationItemDto> Items { get; set; } = new();
    }

    public class QuotationItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountPercent { get; set; }
    }

    public class QuotationResponseDto
    {
        public int QuotationId { get; set; }
        public string QuotationNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? ValidUntil { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal UnitPrice { get; set; }

        public int StockQuantity { get; set; } = 0;
    }

    public class ProductUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal UnitPrice { get; set; }

        public int StockQuantity { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }

    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

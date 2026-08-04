using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CustomerCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Address { get; set; }
        
        [MaxLength(200)]
        public string? Company { get; set; }
    }

    public class CustomerUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Address { get; set; }
        
        [MaxLength(200)]
        public string? Company { get; set; }
        
        public bool IsActive { get; set; } = true;
    }

    public class CustomerResponseDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Company { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class User : BaseEntity
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;


    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = string.Empty;
}
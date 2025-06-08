using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopsicleFactory.AuthApi.Entities;

[Table("user")]
public class User
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

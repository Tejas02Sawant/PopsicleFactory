using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopsicleFactory.DataProvider.Models;

[Table("popsicle")]
public class Popsicle
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public required string Flavor { get; set; }
    public string? Color { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal Price { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastUpdated { get; set; } = DateTime.UtcNow;
}

using System.ComponentModel.DataAnnotations;

namespace PopsicleFactory.WebApi.Models;

public class PopsicleCreateDto
{
    [Required]
    public required string Flavor { get; set; }
    public string? Color { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public decimal Price { get; set; }
}

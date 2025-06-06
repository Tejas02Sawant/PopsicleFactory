namespace PopsicleFactory.WebApi.Models;

public class PopsicleReadDto
{
    public Guid Id { get; set; }
    public required string Flavor { get; set; }
    public string? Color { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

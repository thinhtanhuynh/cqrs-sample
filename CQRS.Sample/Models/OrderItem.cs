namespace CQRS.Sample.Models;

public record OrderItem
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = null!;
    public int Quantity { get; init; }
    public decimal Price { get; init; }
    public Guid OrderId { get; set; }
}
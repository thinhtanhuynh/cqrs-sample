namespace CQRS.Sample.Models;
public record Order
{
    public Guid Id { get; init; }
    public string CustomerName { get; init; } = null!;
    public List<OrderItem> Items { get; init; } = new List<OrderItem>();
    public decimal Total { get; init; }
    public DateTime OrderDate { get; init; }
    public string Status { get; set; } = null!;
}
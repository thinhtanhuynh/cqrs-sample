namespace CQRS.Sample.DTOs;

public record OrderItemDto(
    Guid OrderId,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal Price);
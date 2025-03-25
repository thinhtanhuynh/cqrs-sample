namespace CQRS.Sample.DTOs;

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal Price);
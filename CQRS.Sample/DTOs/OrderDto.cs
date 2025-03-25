namespace CQRS.Sample.DTOs;

public record OrderDto(
    Guid Id,
    string CustomerName,
    List<OrderItemDto> Items,
    decimal Total,
    DateTime OrderDate,
    string Status);
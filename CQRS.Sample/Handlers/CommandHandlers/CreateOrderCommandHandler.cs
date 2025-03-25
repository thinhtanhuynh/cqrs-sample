using CQRS.Sample.Commands;
using CQRS.Sample.Data;
using CQRS.Sample.Models;
using MediatR;

namespace CQRS.Sample.Handlers.CommandHandlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly OrderWriteDbContext _writeContext;

    public CreateOrderCommandHandler(OrderWriteDbContext writeContext)
    {
        _writeContext = writeContext;
    }

    public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        // Input validation
        if (command.Items == null || !command.Items.Any())
        {
            throw new ArgumentException("Order must contain at least one item.");
        }

        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            CustomerName = command.CustomerName,
            OrderDate = DateTime.UtcNow,
            Status = "Pending", // Initial status
            Items = command.Items.Select(itemDto => new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = itemDto.ProductId,
                ProductName = itemDto.ProductName,
                Quantity = itemDto.Quantity,
                Price = itemDto.Price,
            }).ToList(),
            Total = command.Items.Sum(item => item.Price * item.Quantity)
        };

        _writeContext.Orders.Add(order);
        await _writeContext.SaveChangesAsync(cancellationToken);
        return order.Id;
    }
}
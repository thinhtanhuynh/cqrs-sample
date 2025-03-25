using CQRS.Sample.Commands;
using CQRS.Sample.Data;
using CQRS.Sample.Models;
using MediatR;

namespace CQRS.Sample.Handlers.CommandHandlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly OrderDbContext _context;

    public CreateOrderCommandHandler(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        // Input validation - Moved to handler
        if (command.Items == null || !command.Items.Any())
        {
            throw new ArgumentException("Order must contain at least one item.");
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = command.CustomerName,
            OrderDate = DateTime.UtcNow,
            Status = "Pending", // Initial status
            Items = command.Items.Select(itemDto => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = itemDto.ProductId,
                ProductName = itemDto.ProductName, //  Get from Product Service
                Quantity = itemDto.Quantity,
                Price = itemDto.Price,
            }).ToList(),
            Total = command.Items.Sum(item => item.Price * item.Quantity)
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order.Id;
    }
}
using CQRS.Sample.Data;
using CQRS.Sample.DTOs;
using CQRS.Sample.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Sample.Handlers.QueryHandlers;

public class GetOrdersByCustomerQueryHandler : IRequestHandler<GetOrdersByCustomerQuery, IEnumerable<OrderDto>>
{
    private readonly OrderDbContext _context;

    public GetOrdersByCustomerQueryHandler(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.CustomerName == request.CustomerName)
            .ToListAsync(cancellationToken);

        return orders.Select(order => new OrderDto(
            Id: order.Id,
            CustomerName: order.CustomerName,
            OrderDate: order.OrderDate,
            Status: order.Status,
            Total: order.Total,
            Items: order.Items.Select(item => new OrderItemDto(
                ProductId: item.ProductId,
                ProductName: item.ProductName,
                Quantity: item.Quantity,
                Price: item.Price
            )).ToList()
        ));
    }
}
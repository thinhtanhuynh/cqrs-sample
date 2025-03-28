using CQRS.Sample.Database;
using CQRS.Sample.DTOs;
using CQRS.Sample.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Sample.Handlers.QueryHandlers;

public class GetOrdersByCustomerQueryHandler : IRequestHandler<GetOrdersByCustomerQuery, IEnumerable<OrderDto>>
{
    private readonly OrderReadDbContext _readContext;

    public GetOrdersByCustomerQueryHandler(OrderReadDbContext readContext)
    {
        _readContext = readContext;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        var orders = await _readContext.Orders
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
                OrderId: order.Id,
                ProductId: item.ProductId,
                ProductName: item.ProductName,
                Quantity: item.Quantity,
                Price: item.Price
            )).ToList()
        ));
    }
}
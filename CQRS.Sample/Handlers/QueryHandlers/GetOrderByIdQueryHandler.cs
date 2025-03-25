using CQRS.Sample.Data;
using CQRS.Sample.DTOs;
using CQRS.Sample.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Sample.Handlers.QueryHandlers;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly OrderReadDbContext _readContext;

    public GetOrderByIdQueryHandler(OrderReadDbContext readContext)
    {
        _readContext = readContext;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _readContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
        {
            return null;
        }

        var orderDto = new OrderDto(
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
        );
        return orderDto;
    }
}
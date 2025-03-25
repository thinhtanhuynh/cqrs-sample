using CQRS.Sample.Commands;
using CQRS.Sample.Data;
using MediatR;
using CQRS.Sample.Exceptions;
namespace CQRS.Sample.Handlers.CommandHandlers;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
{
    private readonly OrderWriteDbContext _writeContext;

    public UpdateOrderStatusCommandHandler(OrderWriteDbContext writeContext)
    {
        _writeContext = writeContext;
    }

    public async Task Handle(UpdateOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var order = await _writeContext.Orders.FindAsync(command.OrderId);
        if (order is null)
        {
            throw new EntityNotFoundException($"Order with id {command.OrderId} not found"); // Custom Exception
        }

        order.Status = command.Status;
        await _writeContext.SaveChangesAsync(cancellationToken);
    }
}
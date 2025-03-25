using System.ComponentModel.DataAnnotations;
using MediatR;

namespace CQRS.Sample.Commands;
public record UpdateOrderStatusCommand(
    [Required] Guid OrderId,
    [Required] string Status) : IRequest;
using System.ComponentModel.DataAnnotations;
using MediatR;
using CQRS.Sample.DTOs;

namespace CQRS.Sample.Commands;
public record CreateOrderCommand(
    [Required] string CustomerName,
    [Required] List<OrderItemDto> Items) : IRequest<Guid>;

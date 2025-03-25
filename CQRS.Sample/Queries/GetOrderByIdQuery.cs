using System.ComponentModel.DataAnnotations;
using MediatR;
using CQRS.Sample.DTOs;

namespace CQRS.Sample.Queries;
public record GetOrderByIdQuery([Required] Guid OrderId) : IRequest<OrderDto?>;
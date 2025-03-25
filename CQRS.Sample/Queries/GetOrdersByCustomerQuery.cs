using System.ComponentModel.DataAnnotations;
using MediatR;
using CQRS.Sample.DTOs;

namespace CQRS.Sample.Queries;
public record GetOrdersByCustomerQuery([Required] string CustomerName) : IRequest<IEnumerable<OrderDto>>;
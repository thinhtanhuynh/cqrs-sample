using CQRS.Sample.Commands;
using CQRS.Sample.DTOs;
using CQRS.Sample.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CQRS.Sample.Exceptions;

namespace CQRS.Sample.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(OrderDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(orderId));
        if (order is null)
        {
            return NotFound();
        }
        return Ok(order!);
    }

    [HttpGet("customer/{customerName}")]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
    public async Task<IActionResult> GetOrdersByCustomer(string customerName)
    {
        var orders = await _mediator.Send(new GetOrdersByCustomerQuery(customerName));
        return Ok(orders);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        try
        {
            var command = new CreateOrderCommand(
                request.CustomerName,
                request.Items.Select(item =>
                    new OrderItemDto(Guid.NewGuid(), item.ProductId, item.ProductName, item.Quantity, item.Price))
                .ToList());
            var orderId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrderById), new { orderId }, new { OrderId = orderId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{orderId}/status")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] string status)
    {
        try
        {
            await _mediator.Send(new UpdateOrderStatusCommand(orderId, status));
            return NoContent();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
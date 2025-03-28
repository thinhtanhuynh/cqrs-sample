using System.ComponentModel.DataAnnotations;

namespace CQRS.Sample.DTOs;
public class CreateOrderRequest
{
    [Required]
    public string CustomerName { get; set; } = null!;
    [Required]
    public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();

    public class OrderItemRequest

    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
    }

}

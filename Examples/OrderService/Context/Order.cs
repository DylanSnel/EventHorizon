// Order model
namespace OrderService.Context;

public class Order
{
    public int Id { get; set; }
    public required int UserId { get; set; }
    public required int ProductId { get; set; }
    public required string Instructions { get; set; }

}

using MediatR;
using OrderService.Context;

namespace OrderService.Commands;

public class CreateOrderCommand : IRequest<Order>
{
    public required int UserId { get; set; }
    public required int ProductId { get; set; }
    public required string Instructions { get; set; }
}

public class CreateOrderCommandHandler(OrderServiceContext context) : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly OrderServiceContext _context = context;

    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order { UserId = request.UserId, ProductId = request.ProductId, Instructions = request.Instructions };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }
}

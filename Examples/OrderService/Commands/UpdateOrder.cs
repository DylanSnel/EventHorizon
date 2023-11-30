using MediatR;
using OrderService.Context;

namespace OrderService.Commands;

public class UpdateOrderCommand : IRequest<Order>
{
    public int Id { get; set; }
    public required int ProductId { get; set; }
    public required string Instructions { get; set; }
}

public class UpdateOrderCommandHandler(OrderServiceContext context) : IRequestHandler<UpdateOrderCommand, Order>
{
    private readonly OrderServiceContext _context = context;

    public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        order.ProductId = request.ProductId;
        order.Instructions = request.Instructions;

        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);

        return order;
    }
}

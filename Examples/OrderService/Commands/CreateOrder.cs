using EventHorizon;
using MassTransit;
using MediatR;
using OrderService.Context;
using Shared.Events;

namespace OrderService.Commands;

public class CreateOrderCommand : IRequest<Order>
{
    public required int UserId { get; set; }
    public required int ProductId { get; set; }
    public required string Instructions { get; set; }
}

[Handler<CreateOrderCommand>(Description: "Create an order")]
[Produces<OrderConfirmedEvent>]
[ProducesError<OrderConfirmedEvent>(Condition: "When the payment failed (or 1 in 5 chance)")]
public class CreateOrderCommandHandler(OrderServiceContext context, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly OrderServiceContext _context = context;

    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Random random = new Random();
        int number = random.Next(0, 5);
        if (number == 0)
        {
            await publishEndpoint.Publish(new OrderFailedEvent { UserId = request.UserId, ProductId = request.ProductId }, cancellationToken);
            throw new Exception("Order failed.");
        }
        var order = new Order { UserId = request.UserId, ProductId = request.ProductId, Instructions = request.Instructions };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        await publishEndpoint.Publish(new OrderConfirmedEvent { Id = order.Id, UserId = request.UserId, ProductId = request.ProductId }, cancellationToken);
        return order;
    }
}

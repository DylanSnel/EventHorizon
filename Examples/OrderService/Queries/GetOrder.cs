using MediatR;
using OrderService.Context;

namespace OrderService.Queries;

public class GetOrderQuery : IRequest<Order>
{
    public int Id { get; set; }
}

public class GetOrderQueryHandler(OrderServiceContext context) : IRequestHandler<GetOrderQuery, Order>
{
    public async Task<Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await context.Orders.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        return order;
    }
}

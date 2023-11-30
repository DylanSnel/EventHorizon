using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Context;

namespace OrderService.Queries;

public class GetAllOrdersQuery : IRequest<IEnumerable<Order>>;

public class GetAllOrdersQueryHandler(OrderServiceContext context) : IRequestHandler<GetAllOrdersQuery, IEnumerable<Order>>
{
    private readonly OrderServiceContext _context = context;

    public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders.ToListAsync(cancellationToken);
    }
}

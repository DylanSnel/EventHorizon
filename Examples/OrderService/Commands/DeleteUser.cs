using EventHorizon;
using MediatR;
using OrderService.Context;
using Shared.Commands;

namespace OrderService.Commands;
[Handler<DeleteUserCommand>(Description: "Delete all order for userId")]
public class DeleteUserCommandHandler(OrderServiceContext context) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        context.Orders.RemoveRange(context.Orders.Where(c => c.Id == request.Id));
        await context.SaveChangesAsync(cancellationToken);
    }
}

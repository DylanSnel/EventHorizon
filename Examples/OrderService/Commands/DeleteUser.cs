using MassTransit;
using MediatR;
using OrderService.Context;
using Shared.Commands;
using Shared.Events;

namespace OrderService.Commands;
public class DeleteUserCommandHandler(OrderServiceContext context, IPublishEndpoint publishEndpoint) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        context.Orders.RemoveRange(context.Orders.Where(c => c.Id == request.Id));
        await context.SaveChangesAsync(cancellationToken);
        await publishEndpoint.Publish(new UserDeletedEvent { Id = request.Id }, cancellationToken);
    }
}

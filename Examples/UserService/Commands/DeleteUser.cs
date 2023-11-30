using EventHorizon;
using MassTransit;
using MediatR;
using Shared.Commands;
using Shared.Events;
using UserService.Context;

namespace UserService.Commands;
[Handler<DeleteUserCommand>(Description: "Delete a user")]
[Produces<UserDeletedEvent>]
public class DeleteUserCommandHandler(UserServiceContext context, IPublishEndpoint publishEndpoint) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        context.Users.RemoveRange(context.Users.Where(c => c.Id == request.Id));
        await context.SaveChangesAsync(cancellationToken);
        await publishEndpoint.Publish(new UserDeletedEvent { Id = request.Id }, cancellationToken);
    }
}

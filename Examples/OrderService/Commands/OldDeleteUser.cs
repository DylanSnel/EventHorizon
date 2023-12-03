using EventHorizon;
using MediatR;
using OrderService.Context;

namespace OrderService.Commands;
public class OldDeleteUserCommand : IRequest
{
    public required int Id { get; set; }
}



[Handler(Description: "Delete all order for userId", Tags: ["Deletion"])]
public class DeleteUserCommandHandler(OrderServiceContext context) : IRequestHandler<OldDeleteUserCommand>
{
    public async Task Handle(OldDeleteUserCommand request, CancellationToken cancellationToken)
    {
        context.Orders.RemoveRange(context.Orders.Where(c => c.Id == request.Id));
        await context.SaveChangesAsync(cancellationToken);
    }
}

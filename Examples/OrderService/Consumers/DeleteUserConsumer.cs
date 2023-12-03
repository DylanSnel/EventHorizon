using EventHorizon;
using MassTransit;
using MediatR;
using OrderService.Commands;
using Shared.Commands;

namespace OrderService.Consumers;

[MapsAttribute<DeleteUserCommand, OldDeleteUserCommand>(Temporary: true)]
public class DeleteUserConsumer(IMediator mediator) : IConsumer<DeleteUserCommand>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<DeleteUserCommand> context)
    {
        var message = new OldDeleteUserCommand { Id = context.Message.Id };
        await _mediator.Send(message);
    }
}

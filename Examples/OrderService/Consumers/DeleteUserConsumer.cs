using MassTransit;
using MediatR;
using Shared.Commands;

namespace OrderService.Consumers;

public class DeleteUserConsumer(IMediator mediator) : IConsumer<DeleteUserCommand>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<DeleteUserCommand> context)
    {
        await _mediator.Send(context.Message);
    }
}

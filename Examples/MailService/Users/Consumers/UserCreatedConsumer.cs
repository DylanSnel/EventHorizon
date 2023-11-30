using MassTransit;
using MediatR;
using Shared.Events;

namespace MailService.Users.Consumers;

public class UserCreatedConsumer(IMediator mediator) : IConsumer<UserCreatedEvent>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        await _mediator.Send(context.Message);
    }
}

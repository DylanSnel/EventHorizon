using MassTransit;
using MediatR;
using Shared.Events;

namespace MailService.Users.Consumers;

public class UserEmailChangedConsumer(IMediator mediator) : IConsumer<UserDeletedEvent>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<UserDeletedEvent> context)
    {
        await _mediator.Send(context.Message);
    }
}

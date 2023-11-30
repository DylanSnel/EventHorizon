using MassTransit;
using MediatR;
using Shared.Events;

namespace MailService.Orders.Consumers;

public class OrderFailedConsumer(IMediator mediator) : IConsumer<OrderFailedEvent>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<OrderFailedEvent> context)
    {
        await _mediator.Send(context.Message);
    }
}

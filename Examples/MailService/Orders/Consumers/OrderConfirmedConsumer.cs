using MassTransit;
using MediatR;
using Shared.Events;

namespace MailService.Orders.Consumers;

public class OrderConfirmedConsumer(IMediator mediator) : IConsumer<OrderConfirmedEvent>
{
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<OrderConfirmedEvent> context)
    {
        await _mediator.Send(context.Message);
    }
}

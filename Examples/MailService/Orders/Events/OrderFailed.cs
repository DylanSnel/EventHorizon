using EventHorizon;
using MailService.Context;
using MailService.Services;
using MediatR;
using Shared.Events;

namespace MailService.Orders.Events;

[Handler<OrderFailedEvent>(Description: "Send order failed mail")]
public class OrderFailedHander(IMailService mailService, MailServiceContext context) : INotificationHandler<OrderFailedEvent>
{
    public async Task Handle(OrderFailedEvent request, CancellationToken cancellationToken)
    {
        var contact = context.Contacts.First(x => x.UserId == request.UserId);
        await mailService.SendEmail(contact.Email, $"Your order for {request.ProductId} Failed! {contact.Name}");
    }
}


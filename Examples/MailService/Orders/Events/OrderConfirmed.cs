using EventHorizon;
using MailService.Context;
using MailService.Services;
using MediatR;
using Shared.Events;

namespace MailService.Orders.Events;
[Handler<OrderConfirmedEvent>(Description: "Send order confirmation mail")]
public class OrderConfirmedHander(IMailService mailService, MailServiceContext context) : INotificationHandler<OrderConfirmedEvent>
{
    public async Task Handle(OrderConfirmedEvent request, CancellationToken cancellationToken)
    {
        var contact = context.Contacts.First(x => x.UserId == request.UserId);
        await mailService.SendEmail(contact.Email, $"Order {request.Id} Confirmed! {contact.Name}");
    }
}

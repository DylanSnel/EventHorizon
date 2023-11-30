using MailService.Context;
using MailService.Services;
using MediatR;
using Shared.Events;

namespace MailService.Orders.Events;

public class OrderFailedHander(IMailService mailService, MailServiceContext context) : INotificationHandler<OrderFailedEvent>
{
    public async Task Handle(OrderFailedEvent request, CancellationToken cancellationToken)
    {
        var contact = context.Contacts.First(x => x.UserId == request.UserId);
        await mailService.SendEmail(contact.Email, $"Order {request.Id} Failed! {contact.Name}");
    }
}


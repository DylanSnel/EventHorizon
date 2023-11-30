using MailService.Context;
using MailService.Services;
using MediatR;
using Shared.Events;

namespace MailService.Users.Events;

public class UserDeletedHander(IMailService mailService, MailServiceContext context) : INotificationHandler<UserDeletedEvent>
{
    public async Task Handle(UserDeletedEvent request, CancellationToken cancellationToken)
    {
        var contact = context.Contacts.First(c => c.UserId == request.Id);
        context.Contacts.RemoveRange();
        await context.SaveChangesAsync(cancellationToken);
        await mailService.SendEmail(contact.Email, $"You have been deleted! {contact.Name}");
    }
}

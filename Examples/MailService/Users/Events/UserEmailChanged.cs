using EventHorizon;
using MailService.Context;
using MailService.Services;
using MediatR;
using Shared.Events;

namespace MailService.Users.Events;

[Handler<UserEmailChangedEvent>]
public class UserEmailChangedHander(IMailService mailService, MailServiceContext context) : INotificationHandler<UserEmailChangedEvent>
{
    public async Task Handle(UserEmailChangedEvent request, CancellationToken cancellationToken)
    {
        context.Contacts.First(x => x.UserId == request.Id).Email = request.Email;
        await context.SaveChangesAsync(cancellationToken);

        await mailService.SendEmail(request.Email, $"Your email has been changed!");
    }
}

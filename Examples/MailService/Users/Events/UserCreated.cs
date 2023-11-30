using EventHorizon;
using MailService.Context;
using MailService.Services;
using MediatR;
using Shared.Events;

namespace MailService.Users.Events;

[Handler<UserCreatedEvent>(Description: "Create contact and send confirmation mail")]
public class UserCreatedHander(IMailService mailService, MailServiceContext context) : INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent request, CancellationToken cancellationToken)
    {
        context.Add(new Contact { UserId = request.Id, Email = request.Email, Name = request.Name });
        await context.SaveChangesAsync(cancellationToken);
        await mailService.SendEmail(request.Email, $"Welcome to the system! {request.Name}");
    }
}

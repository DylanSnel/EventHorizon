using MailService.Services;
using MediatR;
using Shared.Events;

namespace MailService.Users.Events;

public class UserCreatedHander(IMailService mailService) : INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent request, CancellationToken cancellationToken)
    {

        await mailService.SendEmail(request.Email, $"Welcome to the system! {request.Name}");
    }
}

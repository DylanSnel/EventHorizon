namespace MailService.Services;

public class MailService : IMailService
{
    public Task SendEmail(string email, string name)
    {
        // Send email
        return Task.CompletedTask;
    }
}

namespace MailService.Services;

public interface IMailService
{
    Task SendEmail(string email, string name);
}

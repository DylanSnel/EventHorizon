// User model
namespace MailService.Context;

public class Contact
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

}

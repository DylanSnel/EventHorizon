// User model
namespace UserService.Context;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
}

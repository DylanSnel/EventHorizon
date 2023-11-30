using MediatR;

namespace Shared.Events;
public class UserCreatedEvent : INotification
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}

public class UserEmailChangedEvent : INotification
{
    public required int Id { get; set; }
    public required string Email { get; set; }
}

public class UserDeletedEvent : INotification
{
    public required int Id { get; set; }
}

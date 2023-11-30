using MediatR;

namespace Shared.Events;

public class OrderConfirmedEvent : INotification
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required int ProductId { get; set; }
}

public class OrderFailedEvent : INotification
{
    public required int UserId { get; set; }
    public required int ProductId { get; set; }
}

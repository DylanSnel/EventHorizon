using MediatR;

namespace Shared.Commands;
public class DeleteUserCommand : IRequest
{
    public required int Id { get; set; }
}

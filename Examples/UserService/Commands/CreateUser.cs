using MediatR;
using UserService.Context;

namespace UserService.Commands;

public class CreateUserCommand : IRequest<User>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}

public class CreateUserCommandHandler(UserServiceContext context) : IRequestHandler<CreateUserCommand, User>
{
    private readonly UserServiceContext _context = context;

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User { Name = request.Name, Email = request.Email };
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }
}

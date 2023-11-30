using MediatR;
using UserService.Context;

namespace UserService.Queries;

public class GetUserQuery : IRequest<User>
{
    public int Id { get; set; }
}

public class GetUserQueryHandler(UserServiceContext context) : IRequestHandler<GetUserQuery, User>
{
    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return user;
    }
}

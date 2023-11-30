using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Context;

namespace UserService.Queries;

public class GetAllUsersQuery : IRequest<IEnumerable<User>>;

public class GetAllUsersQueryHandler(UserServiceContext context) : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
{
    private readonly UserServiceContext _context = context;

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users.ToListAsync(cancellationToken);
    }
}

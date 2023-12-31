﻿using EventHorizon;
using MassTransit;
using MediatR;
using Shared.Events;
using UserService.Context;

namespace UserService.Commands;

public class CreateUserCommand : IRequest<User>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}

[Handler<CreateUserCommand>(Description: "Create a user")]
[Produces<UserCreatedEvent>]
public class CreateUserCommandHandler(UserServiceContext context, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateUserCommand, User>
{
    private readonly UserServiceContext _context = context;

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User { Name = request.Name, Email = request.Email };
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        await publishEndpoint.Publish(new UserCreatedEvent { Id = user.Id, Name = user.Name, Email = user.Email }, cancellationToken);
        return user;
    }
}

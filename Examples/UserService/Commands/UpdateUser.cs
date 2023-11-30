﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Context;

namespace UserService.Commands;

public class UpdateUserCommand : IRequest<User>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
}

public class UpdateUserCommandHandler(UserServiceContext context) : IRequestHandler<UpdateUserCommand, User>
{
    private readonly UserServiceContext _context = context;

    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        user.Name = request.Name;

        if (user.Email != request.Email)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken: cancellationToken);
            if (existingUser != null)
            {
                throw new Exception("Email already in use.");
            }
        }

        user.Email = request.Email;
        user.Address = request.Address;
        user.City = request.City;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user;
    }
}

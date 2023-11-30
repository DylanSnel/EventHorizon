using Microsoft.EntityFrameworkCore;

namespace UserService.Context;

public class UserServiceContext(DbContextOptions<UserServiceContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}

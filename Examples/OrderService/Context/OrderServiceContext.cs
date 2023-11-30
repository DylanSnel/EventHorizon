using Microsoft.EntityFrameworkCore;

namespace OrderService.Context;

public class OrderServiceContext(DbContextOptions<OrderServiceContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
}

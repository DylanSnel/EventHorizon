using Microsoft.EntityFrameworkCore;

namespace MailService.Context;

public class MailServiceContext(DbContextOptions<MailServiceContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }
}

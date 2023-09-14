using Microsoft.EntityFrameworkCore;
using NotificationService.Domain;

namespace NotificationService.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Email> Emails { get; set; } = null!;
}
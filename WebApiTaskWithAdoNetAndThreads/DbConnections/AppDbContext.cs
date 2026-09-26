using Microsoft.EntityFrameworkCore;
using WebApiTaskWithAdoNetAndThreads.Models;

namespace WebApiTaskWithAdoNetAndThreads.DbConnections;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
}
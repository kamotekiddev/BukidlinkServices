using Microsoft.EntityFrameworkCore;
using PaymentAPI.Models.PaymentTransaction;

namespace PaymentAPI.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<PaymentTransaction> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
    }
}
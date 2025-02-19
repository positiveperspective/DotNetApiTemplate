using Microsoft.EntityFrameworkCore;
using DotNetAPI.Domain.OrderDomain;

namespace DotNetAPI.Infrastructure.Database;

public class DotNetAPIContext : DbContext
{
  
    public DbSet<Order> Orders { get; set; }//

    public DotNetAPIContext(DbContextOptions<DotNetAPIContext> options) : base(options)
    {
       
    }

    public DotNetAPIContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.Entity<Order>().HasKey("OrderID");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //custom logic
        return base.SaveChangesAsync(cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Product>()
			.Property(p => p.CurrentPrice)
			.HasPrecision(18, 2);

		modelBuilder.Entity<Product>()
			.Property(p => p.OldPrice)
			.HasPrecision(18, 2);
	}
	public DbSet<Product> Products { get; set; }
}
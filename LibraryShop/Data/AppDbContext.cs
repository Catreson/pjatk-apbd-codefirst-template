using LibraryShop.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryShop.Data;

public class AppDbContext : DbContext
{
    // Each DbSet<T> maps to a table in the database.
    // The property name (e.g. "Books") becomes the default table name unless overridden by [Table] attribute.
    public DbSet<Author>      Authors      { get; set; }
    public DbSet<Book>        Books        { get; set; }
    public DbSet<Category>    Categories   { get; set; }
    public DbSet<BookCategory>BookCategories{ get; set; }
    public DbSet<Customer>    Customers    { get; set; }
    public DbSet<Order>       Orders       { get; set; }
    public DbSet<OrderItem>   OrderItems   { get; set; }

    protected AppDbContext() { }

    public AppDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This single line scans the assembly for all classes implementing
        // IEntityTypeConfiguration<T> and applies them automatically.
        // You never need to call modelBuilder.Entity<X>() manually here.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}

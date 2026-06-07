using LibraryShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryShop.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(c => c.LastName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(200).IsRequired();

        // One Customer has many Orders
        builder.HasMany(c => c.Orders)
               .WithOne(o => o.Customer)
               .HasForeignKey(o => o.CustomerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(new List<Customer>
        {
            new Customer { Id = 1, FirstName = "Anna",   LastName = "Kowalska",  Email = "anna.kowalska@example.com" },
            new Customer { Id = 2, FirstName = "Piotr",  LastName = "Nowak",     Email = "piotr.nowak@example.com" },
            new Customer { Id = 3, FirstName = "Maria",  LastName = "Wiśniewska",Email = "maria.w@example.com" },
        });
    }
}

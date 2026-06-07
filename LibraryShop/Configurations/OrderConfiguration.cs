using LibraryShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryShop.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.FulfilledAt).IsRequired(false); // nullable = not fulfilled yet

        // Seed some sample orders
        builder.HasData(new List<Order>
        {
            new Order { Id = 1, CreatedAt = new DateTime(2024, 1, 10), FulfilledAt = new DateTime(2024, 1, 11), CustomerId = 1 },
            new Order { Id = 2, CreatedAt = new DateTime(2024, 2, 15), FulfilledAt = null,                       CustomerId = 1 },
            new Order { Id = 3, CreatedAt = new DateTime(2024, 3, 20), FulfilledAt = null,                       CustomerId = 2 },
        });
    }
}

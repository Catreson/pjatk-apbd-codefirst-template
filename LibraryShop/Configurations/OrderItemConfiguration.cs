using LibraryShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryShop.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        // Composite PK
        builder.HasKey(oi => new { oi.OrderId, oi.BookId });

        builder.Property(oi => oi.Quantity).IsRequired();
        builder.Property(oi => oi.UnitPrice).HasColumnType("numeric(10,2)").IsRequired();

        builder.HasOne(oi => oi.Order)
               .WithMany(o => o.OrderItems)
               .HasForeignKey(oi => oi.OrderId);

        builder.HasOne(oi => oi.Book)
               .WithMany(b => b.OrderItems)
               .HasForeignKey(oi => oi.BookId);

        // Order 1: Anna bought Witcher 1 (x2) and 1984 (x1)
        // Order 2: Anna bought Dune (x1)
        // Order 3: Piotr bought Fellowship (x3)
        builder.HasData(new List<OrderItem>
        {
            new OrderItem { OrderId = 1, BookId = 1, Quantity = 2, UnitPrice = 29.99m },
            new OrderItem { OrderId = 1, BookId = 7, Quantity = 1, UnitPrice = 24.99m },
            new OrderItem { OrderId = 2, BookId = 5, Quantity = 1, UnitPrice = 45.00m },
            new OrderItem { OrderId = 3, BookId = 3, Quantity = 3, UnitPrice = 39.99m },
        });
    }
}

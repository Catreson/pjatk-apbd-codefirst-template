using LibraryShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryShop.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title).HasMaxLength(200).IsRequired();
        builder.Property(b => b.Price).HasColumnType("numeric(10,2)").IsRequired();
        builder.Property(b => b.Stock).IsRequired();
        builder.Property(b => b.PublishedAt).IsRequired();

        // Relationship to Author is configured on AuthorConfiguration side.
        // Relationship to BookCategory is configured on BookCategoryConfiguration side.

        builder.HasData(new List<Book>
        {
            new Book { Id = 1, Title = "The Witcher: Blood of Elves",    Price = 29.99m, Stock = 15, PublishedAt = new DateTime(1994, 1, 1), AuthorId = 1 },
            new Book { Id = 2, Title = "The Witcher: Time of Contempt",  Price = 29.99m, Stock = 10, PublishedAt = new DateTime(1995, 1, 1), AuthorId = 1 },
            new Book { Id = 3, Title = "The Fellowship of the Ring",     Price = 39.99m, Stock = 20, PublishedAt = new DateTime(1954, 7, 29), AuthorId = 2 },
            new Book { Id = 4, Title = "The Two Towers",                 Price = 39.99m, Stock = 18, PublishedAt = new DateTime(1954, 11, 11), AuthorId = 2 },
            new Book { Id = 5, Title = "Dune",                          Price = 45.00m, Stock = 8,  PublishedAt = new DateTime(1965, 8, 1), AuthorId = 3 },
            new Book { Id = 6, Title = "Dune Messiah",                  Price = 40.00m, Stock = 0,  PublishedAt = new DateTime(1969, 1, 1), AuthorId = 3 },
            new Book { Id = 7, Title = "1984",                          Price = 24.99m, Stock = 25, PublishedAt = new DateTime(1949, 6, 8), AuthorId = 4 },
            new Book { Id = 8, Title = "Animal Farm",                   Price = 19.99m, Stock = 30, PublishedAt = new DateTime(1945, 8, 17), AuthorId = 4 },
        });
    }
}

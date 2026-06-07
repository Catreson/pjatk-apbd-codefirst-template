using LibraryShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryShop.Configurations;

// IEntityTypeConfiguration<T> lets you move all configuration for one model
// into its own class instead of cluttering OnModelCreating in AppDbContext.
// AppDbContext calls modelBuilder.ApplyConfigurationsFromAssembly(...) to pick these up automatically.
public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        // --- Column constraints ---
        builder.HasKey(a => a.Id);
        builder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(a => a.LastName).HasMaxLength(100).IsRequired();
        builder.Property(a => a.BirthYear).IsRequired();

        // --- Relationships ---
        // One Author has many Books. The FK (AuthorId) lives on the Book side.
        builder.HasMany(a => a.Books)
               .WithOne(b => b.Author)
               .HasForeignKey(b => b.AuthorId)
               .OnDelete(DeleteBehavior.Restrict); // Don't cascade-delete books when author deleted

        // --- Seed data ---
        builder.HasData(new List<Author>
        {
            new Author { Id = 1, FirstName = "Andrzej",  LastName = "Sapkowski", BirthYear = 1948 },
            new Author { Id = 2, FirstName = "J.R.R.",   LastName = "Tolkien",   BirthYear = 1892 },
            new Author { Id = 3, FirstName = "Frank",    LastName = "Herbert",   BirthYear = 1920 },
            new Author { Id = 4, FirstName = "George",   LastName = "Orwell",    BirthYear = 1903 },
        });
    }
}

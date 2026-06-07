using LibraryShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryShop.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();

        builder.HasData(new List<Category>
        {
            new Category { Id = 1, Name = "Fantasy" },
            new Category { Id = 2, Name = "Science Fiction" },
            new Category { Id = 3, Name = "Dystopia" },
            new Category { Id = 4, Name = "Political Satire" },
            new Category { Id = 5, Name = "Classic" },
        });
    }
}

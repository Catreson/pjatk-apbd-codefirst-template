using LibraryShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryShop.Configurations;

public class BookCategoryConfiguration : IEntityTypeConfiguration<BookCategory>
{
    public void Configure(EntityTypeBuilder<BookCategory> builder)
    {
        // Composite primary key
        builder.HasKey(bc => new { bc.BookId, bc.CategoryId });

        // Relationships: BookCategory is the join table between Book and Category
        builder.HasOne(bc => bc.Book)
               .WithMany(b => b.BookCategories)
               .HasForeignKey(bc => bc.BookId);

        builder.HasOne(bc => bc.Category)
               .WithMany(c => c.BookCategories)
               .HasForeignKey(bc => bc.CategoryId);

        // Seed data - which books belong to which categories
        builder.HasData(new List<BookCategory>
        {
            // Witcher books -> Fantasy
            new BookCategory { BookId = 1, CategoryId = 1 },
            new BookCategory { BookId = 2, CategoryId = 1 },
            // Tolkien books -> Fantasy, Classic
            new BookCategory { BookId = 3, CategoryId = 1 },
            new BookCategory { BookId = 3, CategoryId = 5 },
            new BookCategory { BookId = 4, CategoryId = 1 },
            new BookCategory { BookId = 4, CategoryId = 5 },
            // Dune books -> Science Fiction
            new BookCategory { BookId = 5, CategoryId = 2 },
            new BookCategory { BookId = 6, CategoryId = 2 },
            // 1984 -> Dystopia, Political Satire, Classic
            new BookCategory { BookId = 7, CategoryId = 3 },
            new BookCategory { BookId = 7, CategoryId = 4 },
            new BookCategory { BookId = 7, CategoryId = 5 },
            // Animal Farm -> Political Satire, Classic
            new BookCategory { BookId = 8, CategoryId = 4 },
            new BookCategory { BookId = 8, CategoryId = 5 },
        });
    }
}

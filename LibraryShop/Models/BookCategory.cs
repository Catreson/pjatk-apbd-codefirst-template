using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryShop.Models;

// Many-to-many join table between Book and Category
// Composite primary key: (BookId, CategoryId)
[Table("Book_Category")]
[PrimaryKey(nameof(BookId), nameof(CategoryId))]
public class BookCategory
{
    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }

    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    // Navigation properties
    public Book Book { get; set; } = null!;
    public Category Category { get; set; } = null!;
}

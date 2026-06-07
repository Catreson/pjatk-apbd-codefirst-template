using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryShop.Models;

[Table("Book")]
public class Book
{
    [Key]
    public int Id { get; set; }

    [MaxLength(200)]
    public string Title { get; set; } = null!;

    // Stored as numeric(10,2) in the database - e.g. 29.99
    [Column(TypeName = "numeric(10,2)")]
    [Precision(10, 2)]
    public decimal Price { get; set; }

    public int Stock { get; set; }

    public DateTime PublishedAt { get; set; }

    // Foreign key to Author
    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }

    // Navigation property: many Books belong to one Author
    public Author Author { get; set; } = null!;

    // Navigation property: many Books belong to many Categories (via BookCategory join table)
    public ICollection<BookCategory> BookCategories { get; set; } = [];

    // Navigation property: one Book can appear in many OrderItems
    public ICollection<OrderItem> OrderItems { get; set; } = [];
}

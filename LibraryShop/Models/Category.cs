using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShop.Models;

[Table("Category")]
public class Category
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    // Navigation property: many Categories belong to many Books
    public ICollection<BookCategory> BookCategories { get; set; } = [];
}

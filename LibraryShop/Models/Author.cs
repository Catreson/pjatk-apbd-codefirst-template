using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShop.Models;

[Table("Author")]
public class Author
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    public int BirthYear { get; set; }

    // Navigation property: one Author has many Books
    public ICollection<Book> Books { get; set; } = [];
}

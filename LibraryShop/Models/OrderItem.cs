using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryShop.Models;

// Many-to-many join table between Order and Book
// Composite primary key: (OrderId, BookId)
[Table("Order_Item")]
[PrimaryKey(nameof(OrderId), nameof(BookId))]
public class OrderItem
{
    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; }

    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }

    public int Quantity { get; set; }

    // Price snapshot at time of order - books can change price later
    [Column(TypeName = "numeric(10,2)")]
    public decimal UnitPrice { get; set; }

    // Navigation properties
    public Order Order { get; set; } = null!;
    public Book Book { get; set; } = null!;
}

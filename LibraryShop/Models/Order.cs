using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShop.Models;

[Table("Order")]
public class Order
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    // Nullable: null means order is not yet fulfilled
    public DateTime? FulfilledAt { get; set; }

    [ForeignKey(nameof(Customer))]
    public int CustomerId { get; set; }

    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = [];
}

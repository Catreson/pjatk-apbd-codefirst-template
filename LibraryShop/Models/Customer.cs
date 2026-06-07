using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryShop.Models;

[Table("Customer")]
public class Customer
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [MaxLength(200)]
    public string Email { get; set; } = null!;

    // Navigation property: one Customer has many Orders
    public ICollection<Order> Orders { get; set; } = [];
}

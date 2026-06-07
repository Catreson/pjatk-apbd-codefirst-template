namespace LibraryShop.DTOs;

// ----- GET /api/orders/{id} -----
public class OrderDetailDto
{
    public int       Id          { get; set; }
    public DateTime  CreatedAt   { get; set; }
    public DateTime? FulfilledAt { get; set; }
    public bool      IsFulfilled => FulfilledAt.HasValue; // computed property
    public CustomerSummaryDto Customer { get; set; } = null!;
    public List<OrderItemDto> Items    { get; set; } = [];
    public decimal TotalPrice => Items.Sum(i => i.UnitPrice * i.Quantity); // total cost
}

public class CustomerSummaryDto
{
    public int    Id        { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName  { get; set; } = null!;
    public string Email     { get; set; } = null!;
}

public class OrderItemDto
{
    public string  BookTitle { get; set; } = null!;
    public int     Quantity  { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => UnitPrice * Quantity; // computed
}

// ----- POST /api/orders (create) -----
public class OrderCreateDto
{
    public int CustomerId { get; set; }
    // Each item specifies which book and how many
    public List<OrderItemCreateDto> Items { get; set; } = [];
}

public class OrderItemCreateDto
{
    public int BookId   { get; set; }
    public int Quantity { get; set; }
}

// ----- PUT /api/orders/{id}/fulfill -----
public class OrderFulfillDto
{
    // Currently empty - just the act of calling this endpoint fulfills the order.
    // You could add a "notes" field here, etc.
}

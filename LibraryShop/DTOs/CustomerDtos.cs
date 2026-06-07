namespace LibraryShop.DTOs;

// ----- GET /api/customers/{id} -----
public class CustomerDetailDto
{
    public int    Id        { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName  { get; set; } = null!;
    public string Email     { get; set; } = null!;
    public List<CustomerOrderSummaryDto> Orders { get; set; } = [];
}

public class CustomerOrderSummaryDto
{
    public int       Id          { get; set; }
    public DateTime  CreatedAt   { get; set; }
    public DateTime? FulfilledAt { get; set; }
    public bool      IsFulfilled => FulfilledAt.HasValue;
    public int       ItemCount   { get; set; } // how many distinct book titles
    public decimal   TotalPrice  { get; set; }
}

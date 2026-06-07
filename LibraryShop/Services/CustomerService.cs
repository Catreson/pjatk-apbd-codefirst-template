using LibraryShop.Data;
using LibraryShop.DTOs;
using LibraryShop.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LibraryShop.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _db;

    public CustomerService(AppDbContext db)
    {
        _db = db;
    }

    // GET customer with all their orders and per-order totals
    // Demonstrates: nested Select projections and computed aggregates in LINQ
    public async Task<CustomerDetailDto> GetCustomerByIdAsync(int id)
    {
        var customer = await _db.Customers
            .Select(c => new CustomerDetailDto
            {
                Id        = c.Id,
                FirstName = c.FirstName,
                LastName  = c.LastName,
                Email     = c.Email,
                Orders    = c.Orders
                    .OrderByDescending(o => o.CreatedAt) // newest orders first
                    .Select(o => new CustomerOrderSummaryDto
                    {
                        Id          = o.Id,
                        CreatedAt   = o.CreatedAt,
                        FulfilledAt = o.FulfilledAt,
                        // Count how many distinct book lines are in this order
                        ItemCount   = o.OrderItems.Count(),
                        // Sum(quantity * unitPrice) gives total cost of this order
                        TotalPrice  = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer is null)
            throw new NotFoundException($"Customer with id {id} was not found.");

        return customer;
    }
}

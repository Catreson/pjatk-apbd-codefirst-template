using LibraryShop.Data;
using LibraryShop.DTOs;
using LibraryShop.Exceptions;
using LibraryShop.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryShop.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }

    // -------------------------------------------------------------------------
    // GET order by ID - demonstrates Include + ThenInclude chain
    // -------------------------------------------------------------------------
    public async Task<OrderDetailDto> GetOrderByIdAsync(int id)
    {
        // Project directly into the DTO to avoid loading unnecessary data
        var order = await _db.Orders
            .Select(o => new OrderDetailDto
            {
                Id          = o.Id,
                CreatedAt   = o.CreatedAt,
                FulfilledAt = o.FulfilledAt,
                Customer    = new CustomerSummaryDto
                {
                    Id        = o.Customer.Id,
                    FirstName = o.Customer.FirstName,
                    LastName  = o.Customer.LastName,
                    Email     = o.Customer.Email,
                },
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    BookTitle = oi.Book.Title,
                    Quantity  = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                }).ToList()
            })
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
            throw new NotFoundException($"Order with id {id} was not found.");

        return order;
    }

    // -------------------------------------------------------------------------
    // POST - Create order (uses a DATABASE TRANSACTION)
    //
    // A transaction ensures that EITHER all database changes succeed,
    // OR none of them do. This prevents partial data corruption.
    //
    // Example: if we deduct stock from 3 books and crash on book 2,
    // the transaction rolls back and book 1's stock is restored.
    // -------------------------------------------------------------------------
    public async Task<OrderDetailDto> CreateOrderAsync(OrderCreateDto dto)
    {
        // Validate customer exists
        var customer = await _db.Customers.FindAsync(dto.CustomerId);
        if (customer is null)
            throw new NotFoundException($"Customer with id {dto.CustomerId} was not found.");

        if (!dto.Items.Any())
            throw new BadRequestException("Order must contain at least one item.");

        // Start a transaction - wraps all the database changes below
        using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            var orderItems = new List<OrderItem>();

            foreach (var item in dto.Items)
            {
                if (item.Quantity <= 0)
                    throw new BadRequestException($"Quantity for book {item.BookId} must be at least 1.");

                // Load the book to check stock
                var book = await _db.Books.FindAsync(item.BookId);
                if (book is null)
                    throw new NotFoundException($"Book with id {item.BookId} was not found.");

                if (book.Stock < item.Quantity)
                    throw new BadRequestException(
                        $"Not enough stock for '{book.Title}'. Available: {book.Stock}, requested: {item.Quantity}.");

                // Deduct stock
                book.Stock -= item.Quantity;

                // Snapshot the price at the time of the order
                orderItems.Add(new OrderItem
                {
                    BookId    = item.BookId,
                    Quantity  = item.Quantity,
                    UnitPrice = book.Price // price captured NOW, even if book price changes later
                });
            }

            var order = new Order
            {
                CreatedAt  = DateTime.UtcNow,
                CustomerId = dto.CustomerId,
                OrderItems = orderItems
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync(); // saves both the order AND the stock deductions

            await transaction.CommitAsync(); // confirms all changes permanently

            return await GetOrderByIdAsync(order.Id);
        }
        catch
        {
            // If anything above threw an exception, roll back ALL changes
            await transaction.RollbackAsync();
            throw; // re-throw so the controller can return the correct HTTP error
        }
    }

    // -------------------------------------------------------------------------
    // PUT - Fulfill an order (uses transaction)
    // -------------------------------------------------------------------------
    public async Task FulfillOrderAsync(int id)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                throw new NotFoundException($"Order with id {id} was not found.");

            if (order.FulfilledAt is not null)
                throw new ConflictException($"Order {id} is already fulfilled.");

            order.FulfilledAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

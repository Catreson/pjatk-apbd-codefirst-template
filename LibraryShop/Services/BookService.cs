using LibraryShop.Data;
using LibraryShop.DTOs;
using LibraryShop.Exceptions;
using LibraryShop.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryShop.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _db;

    // AppDbContext is injected by ASP.NET Core's dependency injection system.
    // You register it in Program.cs with builder.Services.AddDbContext<AppDbContext>(...).
    public BookService(AppDbContext db)
    {
        _db = db;
    }

    // -------------------------------------------------------------------------
    // GET ALL with optional LINQ filters
    // -------------------------------------------------------------------------
    public async Task<IEnumerable<BookListItemDto>> GetAllBooksAsync(
        string?  authorLastName,
        decimal? minPrice,
        decimal? maxPrice,
        string?  category,
        bool?    inStock)
    {
        // Start with IQueryable - nothing is sent to the database yet.
        // IQueryable builds up a query expression tree that EF Core translates to SQL.
        var query = _db.Books
            .Include(b => b.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsQueryable();

        // --- LINQ filtering ---
        // Each .Where() call adds an AND condition to the SQL query.
        // These are only evaluated when we call ToListAsync() at the end.

        // Filter by author's last name (case-insensitive contains)
        if (!string.IsNullOrWhiteSpace(authorLastName))
            query = query.Where(b => b.Author.LastName.Contains(authorLastName));

        // Filter by minimum price
        if (minPrice.HasValue)
            query = query.Where(b => b.Price >= minPrice.Value);

        // Filter by maximum price
        if (maxPrice.HasValue)
            query = query.Where(b => b.Price <= maxPrice.Value);

        // Filter by category name (book must belong to that category)
        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(b => b.BookCategories.Any(bc => bc.Category.Name == category));

        // Filter only in-stock books
        if (inStock == true)
            query = query.Where(b => b.Stock > 0);

        // .Select() projects the Book entity into a DTO.
        // This is called "projection" - instead of loading entire objects, we only
        // fetch the columns we actually need. Much more efficient!
        return await query
            .OrderBy(b => b.Title) // alphabetical order
            .Select(b => new BookListItemDto
            {
                Id          = b.Id,
                Title       = b.Title,
                Price       = b.Price,
                Stock       = b.Stock,
                PublishedAt = b.PublishedAt,
                // Combine first and last name into one string
                AuthorName  = b.Author.FirstName + " " + b.Author.LastName,
                // Select just the category names from the join table
                Categories  = b.BookCategories.Select(bc => bc.Category.Name).ToList()
            })
            .ToListAsync(); // THIS is when SQL is executed and data comes from the database
    }

    // -------------------------------------------------------------------------
    // GET single book by ID
    // -------------------------------------------------------------------------
    public async Task<BookDetailDto> GetBookByIdAsync(int id)
    {
        // Option A: project directly with .Select() - most efficient, no extra round-trips
        var book = await _db.Books
            .Select(b => new BookDetailDto
            {
                Id          = b.Id,
                Title       = b.Title,
                Price       = b.Price,
                Stock       = b.Stock,
                PublishedAt = b.PublishedAt,
                Author      = new AuthorDto
                {
                    Id        = b.Author.Id,
                    FirstName = b.Author.FirstName,
                    LastName  = b.Author.LastName,
                    BirthYear = b.Author.BirthYear,
                },
                Categories  = b.BookCategories.Select(bc => bc.Category.Name).ToList()
            })
            .FirstOrDefaultAsync(b => b.Id == id); // filter AFTER projection

        if (book is null)
            throw new NotFoundException($"Book with id {id} was not found.");

        return book;
    }

    // -------------------------------------------------------------------------
    // POST - create new book
    // -------------------------------------------------------------------------
    public async Task<BookDetailDto> CreateBookAsync(BookCreateDto dto)
    {
        // Validate that the author exists before creating
        var authorExists = await _db.Authors.AnyAsync(a => a.Id == dto.AuthorId);
        if (!authorExists)
            throw new NotFoundException($"Author with id {dto.AuthorId} does not exist.");

        // Validate all category IDs exist
        var validCategoryIds = await _db.Categories
            .Where(c => dto.CategoryIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        var invalidIds = dto.CategoryIds.Except(validCategoryIds).ToList();
        if (invalidIds.Any())
            throw new NotFoundException($"Categories with ids {string.Join(", ", invalidIds)} do not exist.");

        // Build the new Book entity from the DTO
        var book = new Book
        {
            Title       = dto.Title,
            Price       = dto.Price,
            Stock       = dto.Stock,
            PublishedAt = dto.PublishedAt,
            AuthorId    = dto.AuthorId,
            // Create the join table rows for each category
            BookCategories = dto.CategoryIds.Select(cId => new BookCategory { CategoryId = cId }).ToList()
        };

        _db.Books.Add(book);
        await _db.SaveChangesAsync(); // saves the INSERT to the database

        // Return the full detail DTO by re-fetching from the DB (so we get the generated Id)
        return await GetBookByIdAsync(book.Id);
    }

    // -------------------------------------------------------------------------
    // PUT - update existing book
    // -------------------------------------------------------------------------
    public async Task UpdateBookAsync(int id, BookUpdateDto dto)
    {
        // FindAsync looks up the primary key - very efficient
        var book = await _db.Books
            .Include(b => b.BookCategories)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null)
            throw new NotFoundException($"Book with id {id} was not found.");

        var authorExists = await _db.Authors.AnyAsync(a => a.Id == dto.AuthorId);
        if (!authorExists)
            throw new NotFoundException($"Author with id {dto.AuthorId} does not exist.");

        // Update the scalar properties
        book.Title       = dto.Title;
        book.Price       = dto.Price;
        book.Stock       = dto.Stock;
        book.PublishedAt = dto.PublishedAt;
        book.AuthorId    = dto.AuthorId;

        // Replace categories: remove old ones, add new ones
        book.BookCategories.Clear();
        foreach (var cId in dto.CategoryIds)
            book.BookCategories.Add(new BookCategory { BookId = id, CategoryId = cId });

        await _db.SaveChangesAsync();
    }

    // -------------------------------------------------------------------------
    // DELETE book
    // -------------------------------------------------------------------------
    public async Task DeleteBookAsync(int id)
    {
        var book = await _db.Books.FindAsync(id);
        if (book is null)
            throw new NotFoundException($"Book with id {id} was not found.");

        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
    }
}

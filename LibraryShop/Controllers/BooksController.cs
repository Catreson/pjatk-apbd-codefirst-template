using LibraryShop.DTOs;
using LibraryShop.Exceptions;
using LibraryShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryShop.Controllers;

// [ApiController] enables automatic model validation, binding from JSON, and more.
// [Route("api/[controller]")] means this controller responds to /api/books
// [controller] is replaced with the class name without "Controller" suffix.
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    // Constructor injection - ASP.NET Core provides the IBookService automatically
    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET /api/books
    // GET /api/books?authorLastName=Tolkien
    // GET /api/books?minPrice=20&maxPrice=40
    // GET /api/books?category=Fantasy&inStock=true
    // All query parameters are optional - if omitted, they don't filter
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string?  authorLastName,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] string?  category,
        [FromQuery] bool?    inStock)
    {
        var books = await _bookService.GetAllBooksAsync(authorLastName, minPrice, maxPrice, category, inStock);
        return Ok(books); // 200 OK with JSON body
    }

    // GET /api/books/3
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return Ok(book);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message); // 404 with error message
        }
    }

    // POST /api/books
    // Body: { "title": "...", "price": 29.99, "stock": 10, "publishedAt": "...", "authorId": 1, "categoryIds": [1,2] }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookCreateDto dto)
    {
        try
        {
            var created = await _bookService.CreateBookAsync(dto);
            // 201 Created with Location header pointing to the new resource
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    // PUT /api/books/3
    // Body: { "title": "...", "price": 35.00, "stock": 5, "publishedAt": "...", "authorId": 1, "categoryIds": [1] }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDto dto)
    {
        try
        {
            await _bookService.UpdateBookAsync(id, dto);
            return Ok(); // 200 OK (no body)
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    // DELETE /api/books/3
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent(); // 204 No Content - standard for successful DELETE
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}

using LibraryShop.DTOs;

namespace LibraryShop.Services;

public interface IBookService
{
    // Returns all books, with optional filters passed as query parameters
    Task<IEnumerable<BookListItemDto>> GetAllBooksAsync(
        string?  authorLastName, // filter by author surname
        decimal? minPrice,       // filter price >= minPrice
        decimal? maxPrice,       // filter price <= maxPrice
        string?  category,       // filter by category name
        bool?    inStock);       // filter only books with Stock > 0

    Task<BookDetailDto>  GetBookByIdAsync(int id);
    Task<BookDetailDto>  CreateBookAsync(BookCreateDto dto);
    Task                 UpdateBookAsync(int id, BookUpdateDto dto);
    Task                 DeleteBookAsync(int id);
}

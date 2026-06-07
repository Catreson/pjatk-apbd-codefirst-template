namespace LibraryShop.DTOs;

// DTO = Data Transfer Object
// We use DTOs so we never expose our database Model classes directly to the API caller.
// This lets us shape the response exactly how we want it.

// ----- GET /api/books (list) -----
public class BookListItemDto
{
    public int      Id          { get; set; }
    public string   Title       { get; set; } = null!;
    public decimal  Price       { get; set; }
    public int      Stock       { get; set; }
    public string   AuthorName  { get; set; } = null!; // "FirstName LastName" combined
    public DateTime PublishedAt { get; set; }
    public List<string> Categories { get; set; } = [];
}

// ----- GET /api/books/{id} (single book with full details) -----
public class BookDetailDto
{
    public int      Id          { get; set; }
    public string   Title       { get; set; } = null!;
    public decimal  Price       { get; set; }
    public int      Stock       { get; set; }
    public DateTime PublishedAt { get; set; }
    public AuthorDto Author     { get; set; } = null!;
    public List<string> Categories { get; set; } = [];
}

public class AuthorDto
{
    public int    Id        { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName  { get; set; } = null!;
    public int    BirthYear { get; set; }
}

// ----- POST /api/books (create) -----
public class BookCreateDto
{
    public string   Title       { get; set; } = null!;
    public decimal  Price       { get; set; }
    public int      Stock       { get; set; }
    public DateTime PublishedAt { get; set; }
    public int      AuthorId    { get; set; }
    // List of category IDs to assign to this book
    public List<int> CategoryIds { get; set; } = [];
}

// ----- PUT /api/books/{id} (update) -----
public class BookUpdateDto
{
    public string   Title       { get; set; } = null!;
    public decimal  Price       { get; set; }
    public int      Stock       { get; set; }
    public DateTime PublishedAt { get; set; }
    public int      AuthorId    { get; set; }
    public List<int> CategoryIds { get; set; } = [];
}

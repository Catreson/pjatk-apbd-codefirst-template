namespace LibraryShop.Exceptions;

// Thrown when input is structurally valid but logically wrong.
// e.g. ordering 0 quantity, or ordering an out-of-stock book.
// The controller catches this and returns 400 Bad Request.
public class BadRequestException : Exception
{
    public BadRequestException() { }
    public BadRequestException(string message) : base(message) { }
    public BadRequestException(string message, Exception inner) : base(message, inner) { }
}

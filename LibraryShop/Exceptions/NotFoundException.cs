namespace LibraryShop.Exceptions;

// Thrown when a requested resource does not exist in the database.
// The controller catches this and returns 404 Not Found.
public class NotFoundException : Exception
{
    public NotFoundException() { }
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string message, Exception inner) : base(message, inner) { }
}

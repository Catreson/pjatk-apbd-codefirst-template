namespace LibraryShop.Exceptions;

// Thrown when the requested operation would violate business rules.
// e.g. trying to fulfill an already-fulfilled order.
// The controller catches this and returns 409 Conflict.
public class ConflictException : Exception
{
    public ConflictException() { }
    public ConflictException(string message) : base(message) { }
    public ConflictException(string message, Exception inner) : base(message, inner) { }
}

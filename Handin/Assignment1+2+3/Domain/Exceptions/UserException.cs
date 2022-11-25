namespace Domain.Exceptions;

public class UserException :Exception
{
    public override string Message { get; }
    public UserException() { }

    public UserException(string message)
        : base(message)
    {
        Message = message;
    }

    public UserException(string message, Exception inner)
        : base(message, inner) {Message = message; }
    public UserException(string message, string userName)
        : this(message)
    {
        Message = message;
    }

    public override string ToString()
    {
        return Message;
    }
}
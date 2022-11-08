namespace Domain.Exceptions;

public class PostException :Exception
{
    public string Title { get; }
    public PostException() { }

    public PostException(string message)
        : base(message) { }

    public PostException(string message, Exception inner)
        : base(message, inner) { }
    public PostException(string message, string title)
        : this(message)
    {
        Title = title;
    }
}
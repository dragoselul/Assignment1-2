namespace Shared.Models;

public class Post
{
    public int Id { get; set; }
    public User owner { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string? Comments { get; set; }
    public int? upDownVote { get; set; }
    
    public Post(User owner, string title, string body)
    {
        this.owner = owner;
        Title = title;
        Body = body;
    }
}
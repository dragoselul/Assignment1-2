namespace Shared.Dtos;

public class PostBasicDto
{
    public int Id { get; init; }
    public string ownerName { get; init; }
    public string Title { get; init; }
    public string Body { get; init; }
    public string? Comments { get; init; }
    public int? upDownVote { get; init; }
}
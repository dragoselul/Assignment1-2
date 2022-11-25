namespace Shared.Dtos;

public class PostUpdateDto
{
    public int Id { get; init; }
    public int? OwnerId { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
}
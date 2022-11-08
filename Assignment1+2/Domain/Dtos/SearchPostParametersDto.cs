namespace Shared.Dtos;

public class SearchPostParametersDto
{
    public string? Username { get; init; }
    public int? userId { get; init; }
    public string? TitleContains { get; init; }
}
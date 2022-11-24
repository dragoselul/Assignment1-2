namespace Shared.Dtos;

public class SearchUserParametersDto
{
    public int? idContains { get; init; }
    public string? UsernameContains { get; init; }
}
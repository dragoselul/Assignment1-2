using Shared.Dtos;
using Shared.Models;

namespace HttpClients.ClientInterfaces;

public interface IPostService
{
    Task CreateAsync(PostCreationDto dto);
    Task<ICollection<Post>> GetAsync(int? id, int? userId, string? title, string? body);
    Task UpdateAsync(PostUpdateDto dto);
    Task<PostBasicDto> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}
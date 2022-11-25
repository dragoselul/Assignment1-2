using Shared.Dtos;
using Shared.Models;

namespace Application.DAOInterfaaces;

public interface IPostDao
{
    Task<Post> CreateAsync(Post post);
    Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters);
    Task UpdateAsync(Post post);
    Task<Post?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}
using Shared.Dtos;
using Shared.Models;

namespace Application.DAOInterfaaces;

public interface IUserDao
{
    Task<User> RegisterAsync(User user);
    Task<User?> GetByUsernameAsync(string userName);
    Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters);
    Task<User?> GetByIdAsync(int id);

}
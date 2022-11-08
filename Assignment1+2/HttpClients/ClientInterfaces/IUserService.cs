using Shared.Dtos;
using Shared.Models;

namespace HttpClients.ClientInterfaces;

public interface IUserService
{
    Task<User> Create(UserRegisterDto dto);
    Task<IEnumerable<User>> GetUsers(string? usernameContains);

    Task<User> GetUser(int? idContains);
    
    Task<User> GetUser(string? usernameContains);
}
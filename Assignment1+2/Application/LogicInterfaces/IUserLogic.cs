using Shared.Dtos;
using Shared.Models;

namespace Application.Logic;

public interface IUserLogic
{
    Task<User> RegisterAsync(UserRegisterDto userRegistrationDto);
    Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters);

    Task<User> GetByIdAsync(SearchUserParametersDto id);

}
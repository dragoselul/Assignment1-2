using Application.DAOInterfaaces;
using Domain.Exceptions;
using Shared.Dtos;
using Shared.Models;

namespace Application.Logic;

public class UserLogic : IUserLogic
{
    private readonly IUserDao userDao;

    public UserLogic(IUserDao userDao)
    {
        this.userDao = userDao;
    }

    public async Task<User> RegisterAsync(UserRegisterDto userToRegister)
    {
        User? existing = await userDao.GetByUsernameAsync(userToRegister.Username);
        if (existing != null)
            throw new UserException("There is already a user with this username", userToRegister.Username);
        ValidateData(userToRegister);
        User toRegister = new User
        {
            Username = userToRegister.Username,
            Password = userToRegister.Password,
            Email = userToRegister.Email
        };
        User registered = await userDao.RegisterAsync(toRegister);
        return registered;
    }
    
    private static void ValidateData(UserRegisterDto userToRegister)
    {
        string userName = userToRegister.Username;
        string password = userToRegister.Password;

        if (userName.Length < 3)
            throw new UserException("Username must be at least 3 characters!",userName);

        if (userName.Length > 15)
            throw new UserException("Username must be less than 16 characters!",userName);
        if(password.Length <3)
            throw new UserException("Password must be at least 3 characters!");
        if (password.Length > 20)
            throw new UserException("Password must be less than 20 characters!");

    }
    
    public Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters)
    {
        return userDao.GetAsync(searchParameters);
    }

    public Task<User> GetByIdAsync(SearchUserParametersDto id)
    {
        return userDao.GetByIdAsync((int)id.idContains);
    }
}
using Application.DAOInterfaaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Dtos;
using Shared.Models;

namespace EfcDataAccess.DAOs;

public class UserEfcDao : IUserDao
{
    private readonly RedditContext Context;

    public UserEfcDao(RedditContext context)
    {
        Context = context;
    }
    
    public async Task<User> RegisterAsync(User user)
    {
        EntityEntry<User> newUser = await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
        return newUser.Entity;
    }

    public async Task<User?> GetByUsernameAsync(string userName)
    {
        User? existing = await Context.Users.FirstOrDefaultAsync(u =>
            u.Username.ToLower().Equals(userName.ToLower())
        );
        return existing;
    }

    public Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters)
    {
        IEnumerable<User> users = Context.Users.AsEnumerable();
        if (searchParameters.UsernameContains != null)
        {
            users = Context.Users.Where(u => u.Username.ToLower().Contains(searchParameters.UsernameContains.ToLower()));
        }

        return Task.FromResult(users);
    }

    public Task<User?> GetByIdAsync(int id)
    {
        User? existing = Context.Users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(existing);
    }
}
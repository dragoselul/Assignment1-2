using System.ComponentModel.DataAnnotations;
using Domain.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Shared.Dtos;
using Shared.Models;

namespace WebApi.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient Client = new();
    
    public async Task<User> ValidateUser(string username, string password)
    {  
        var query = new Dictionary<string, string>()
        {
            ["username"] = username
        };
        var uri = QueryHelpers.AddQueryString("https://localhost:7130/users", query);
        HttpResponseMessage response = await Client.GetAsync(uri);
        string content = response.Content.ReadAsStringAsync().Result;
        List<User>? users = JsonConvert.DeserializeObject<List<User>>(content);
        User? user = users[0];
        if (user is null)
        {
            throw new Exception("User not found");
        }

        if (!user.Password.Equals(password))
        {
            throw new Exception("Password mismatch");
        }

        return await Task.FromResult(user);
    }

    public async Task RegisterUser(UserRegisterDto user)
    {
        var query = new Dictionary<string, string>()
        {
            ["username"] = user.Username
        };
        var uri = QueryHelpers.AddQueryString("https://localhost:7130/Users", query);
        HttpResponseMessage response = await Client.GetAsync(uri);
        string content = response.Content.ReadAsStringAsync().Result;
        List<User>? users = new();
        try
        {
            users = JsonConvert.DeserializeObject<List<User>>(content);
        }
        catch (Exception e)
        {
            Console.WriteLine(content);
        }

        if (users.Count > 0)
        {
            throw new UserException("User already exists");
        }
        if (string.IsNullOrEmpty(user.Username))
        {
            throw new UserException("Username cannot be null");
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            throw new UserException("Password cannot be null");
        }
        
        await Client.PostAsJsonAsync("https://localhost:7130/users", user);
    }
}
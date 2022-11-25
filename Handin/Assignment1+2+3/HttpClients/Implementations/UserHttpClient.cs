using System.Net.Http.Json;
using System.Text.Json;
using HttpClients.ClientInterfaces;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Dtos;
using Shared.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HttpClients.Implementations;

public class UserHttpClient : IUserService
{
    private readonly HttpClient client;

    public UserHttpClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<User> Create(UserRegisterDto dto)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("/users", dto);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        User user = JsonSerializer.Deserialize<User>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return user;
    }

    private static string ConstructQuery(string? username)
    {
        string query = "";
        if (!string.IsNullOrEmpty(username))
        {
            query += $"?username={username}";
        }

        return query;
    }
    public async Task<IEnumerable<User>> GetUsers(string? usernameContains = null)
    {
        string query = ConstructQuery(usernameContains);
        HttpResponseMessage response = await client.GetAsync("https://localhost:7130/Users" + query);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        IEnumerable<User> users = JsonSerializer.Deserialize<IEnumerable<User>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return users;
    }

    public async Task<User> GetUser(int? idContains = null)
    {
        var uri = $"https://localhost:7130/users/{idContains}";
        HttpResponseMessage response = await client.GetAsync(uri);
        string content = response.Content.ReadAsStringAsync().Result;
        List<User>? users = JsonConvert.DeserializeObject<List<User>>(content);
        User? user = users[0];
        if (user is null)
        {
            throw new Exception("User not found");
        }

        return await Task.FromResult(user);
    }

    public async Task<User> GetUser(string? usernameContains)
    {
        var query = new Dictionary<string, string>()
        {
            ["username"] = usernameContains
        };
        var uri = QueryHelpers.AddQueryString("https://localhost:7130/users", query);
        HttpResponseMessage response = await client.GetAsync(uri);
        string content = response.Content.ReadAsStringAsync().Result;
        List<User>? users = JsonConvert.DeserializeObject<List<User>>(content);
        User? user = users[0];
        if (user is null)
        {
            throw new Exception("User not found");
        }

        return await Task.FromResult(user);
    }
}
using Shared.Models;

namespace FileData;

public class DataContainer
{
    public ICollection<Post> Posts { get; set; }
    public ICollection<User> Users { get; set; }
}
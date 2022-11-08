using Application.DAOInterfaaces;
using Domain.Exceptions;
using Shared.Dtos;
using Shared.Models;

namespace FileData.DAOs;

public class PostFileDao : IPostDao
{
    private readonly FileContext context;

    public PostFileDao(FileContext context)
    {
        this.context = context;
    }

    public Task<Post> CreateAsync(Post post)
    {
        int id = 1;
        if (context.Posts.Any())
        {
            id = context.Posts.Max(t => t.Id);
            id++;
        }

        post.Id = id;

        context.Posts!.Add(post);
        context.SaveChanges();

        return Task.FromResult(post);
    }

    public Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters)
    {
        IEnumerable<Post> posts = context.Posts.AsEnumerable();
        if (!string.IsNullOrEmpty(searchParameters.Username))
        {
            posts = context.Posts.Where(post =>
                post.owner.Username.Equals(searchParameters.Username, StringComparison.OrdinalIgnoreCase));
        }

        if (searchParameters.userId != null)
        {
            posts = posts.Where(p => p.owner.Id == searchParameters.userId);
        }

        if (!string.IsNullOrEmpty(searchParameters.TitleContains))
        {
            posts = posts.Where(p =>
                p.Title.Contains(searchParameters.TitleContains, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(posts);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existing = context.Posts.FirstOrDefault(p => p.Id == post.Id);
        if (existing == null)
        {
            throw new PostException($"Post with id {post.Id} does not exist!");
        }

        context.Posts.Remove(existing);
        context.Posts.Add(post);

        context.SaveChanges();

        return Task.CompletedTask;
    }

    public Task<Post?> GetByIdAsync(int id)
    {
        Post? post = context.Posts.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(post);
    }

    public Task DeleteAsync(int id)
    {
        Post? existing = context.Posts.FirstOrDefault(p => p.Id == id);
        if (existing == null)
        {
            throw new PostException($"Post with id {id} does not exist!");
        }

        context.Posts.Remove(existing);
        context.SaveChanges();

        return Task.CompletedTask;
    }
}
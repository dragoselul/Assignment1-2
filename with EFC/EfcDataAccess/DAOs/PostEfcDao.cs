using Application.DAOInterfaaces;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Dtos;
using Shared.Models;

namespace EfcDataAccess.DAOs;

public class PostEfcDao : IPostDao
{
    
    private readonly RedditContext Context;

    public PostEfcDao(RedditContext context)
    {
        Context = context;
    }
    
    public async Task<Post> CreateAsync(Post post)
    {
        EntityEntry<Post> newPost = await Context.Posts.AddAsync(post);
        await Context.SaveChangesAsync();
        return newPost.Entity;
    }

    public Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters)
    {
        IEnumerable<Post> posts = Context.Posts.AsEnumerable();
        if (!string.IsNullOrEmpty(searchParameters.Username))
        {
            posts = Context.Posts.Where(post =>
                post.owner.Username.ToLower().Equals(searchParameters.Username.ToLower()));
        }

        if (searchParameters.userId != null)
        {
            posts = posts.Where(p => p.owner.Id == searchParameters.userId);
        }

        if (!string.IsNullOrEmpty(searchParameters.TitleContains))
        {
            posts = posts.Where(p =>
                p.Title.ToLower().Contains(searchParameters.TitleContains.ToLower()));
        }

        return Task.FromResult(posts);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existing = Context.Posts.FirstOrDefault(p => p.Id == post.Id);
        if (existing == null)
        {
            throw new PostException($"Post with id {post.Id} does not exist!");
        }

        Context.Posts.Remove(existing);
        Context.Posts.Add(post);
        Context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task<Post?> GetByIdAsync(int id)
    {
        Post? post = Context.Posts.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(post);
    }

    public Task DeleteAsync(int id)
    {
        Post? existing = Context.Posts.FirstOrDefault(p => p.Id == id);
        if (existing == null)
        {
            throw new PostException($"Post with id {id} does not exist!");
        }

        Context.Posts.Remove(existing);
        Context.SaveChanges();

        return Task.CompletedTask;
    }
}
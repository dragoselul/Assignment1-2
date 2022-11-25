using Application.DAOInterfaaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Dtos;
using Shared.Models;

namespace EfcDataAccess.DAOs;

public class PostEfcDao : IPostDao
{
    private readonly RedditContext context;

    public PostEfcDao(RedditContext context)
    {
        this.context = context;
    }

    public async Task<Post> CreateAsync(Post post)
    {
        EntityEntry<Post> added = await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters)
    {
        IQueryable<Post> query = context.Posts.Include(post => post.owner).AsQueryable();
        if (!string.IsNullOrEmpty(searchParameters.Username))
        {
            // we know username is unique, so just fetch the first
            query = query.Where(post =>
                post.owner.Username.Equals(searchParameters.Username, StringComparison.OrdinalIgnoreCase));
        }
    
        if (searchParameters.userId != null)
        {
            query = query.Where(p => p.owner.Id == searchParameters.userId);
        }

        if (!string.IsNullOrEmpty(searchParameters.TitleContains))
        {
            query = query.Where(p =>
                p.Title.Contains(searchParameters.TitleContains, StringComparison.OrdinalIgnoreCase));
        }

        List<Post> result = await query.ToListAsync();
        return result;
    }

    public async Task UpdateAsync(Post post)
    {
        context.ChangeTracker.Clear();
        context.Posts.Update(post);
        await context.SaveChangesAsync();
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        Post? found = await context.Posts
            .AsNoTracking()
            .Include(post => post.owner)
            .SingleOrDefaultAsync(post => post.Id == id);
        return found;
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Post with id {id} not found");
        }

        context.Posts.Remove(existing);
        await context.SaveChangesAsync();
    }
}
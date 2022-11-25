using Application.DAOInterfaaces;
using Domain.Exceptions;
using Shared.Dtos;
using Shared.Models;

namespace Application.Logic;

public class PostLogic : IPostLogic
{
    private readonly IUserDao userDao;
    private readonly IPostDao postDao;

    public PostLogic(IUserDao userDao, IPostDao postDao)
    {
        this.userDao = userDao;
        this.postDao = postDao;
    }

    public async Task<Post> CreateAsync(PostCreationDto dto)
    {
        User? user = await userDao.GetByIdAsync(dto.OwnerId);
        if (user == null)
            throw new UserException("The user does not exist!");
        ValidatePost(dto);
        Post post = new Post(user, dto.Title, dto.Body);
        Post created = await postDao.CreateAsync(post);
        return created;
    }
    
    private void ValidatePost(PostCreationDto dto)
    {
        if (string.IsNullOrEmpty(dto.Title)) throw new PostException("Title cannot be empty.");
    }

    public Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters)
    {
        return postDao.GetAsync(searchParameters);
    }

    public async Task UpdateAsync(PostUpdateDto dto)
    {
        Post? existing = await postDao.GetByIdAsync(dto.Id);
        if (existing == null)
            throw new PostException("Not an existing post!");
        User? user = null;
        if (dto.OwnerId != null)
        {
            user = await userDao.GetByIdAsync((int)dto.OwnerId);
            if (user == null)
                throw new UserException("No user with this ID!", $"{dto.OwnerId}");
            
        }

        User userToUse = user ?? existing.owner;
        string titleToUse = dto.Title ?? existing.Title;
        string bodyToUse = dto.Body ?? existing.Body;
        Post updated = new(userToUse, titleToUse, bodyToUse)
        {
            Id = existing.Id,
            Comments = existing.Comments,
            upDownVote = existing.upDownVote
        };
        
        ValidatePost(updated);

        await postDao.UpdateAsync(updated);
    }
    
    private void ValidatePost(Post dto)
    {
        if (string.IsNullOrEmpty(dto.Title)) throw new PostException("Title cannot be empty.");
    }

    public async Task DeleteAsync(int id)
    {
        Post? post = await postDao.GetByIdAsync(id);
        if (post == null)
        {
            throw new PostException($"Post with ID {id} was not found!");
        }

        await postDao.DeleteAsync(id);
    }

    public async Task<PostBasicDto> GetByIdAsync(int id)
    {
        Post? post = await postDao.GetByIdAsync(id);
        if (post == null)
        {
            throw new PostException($"Post with id {id} not found");
        }

        PostBasicDto postBasicDto = new()
        {
            Id = post.Id,
            ownerName = post.owner.Username,
            Title = post.Title,
            Body = post.Body,
            Comments = post.Comments,
            upDownVote = post.upDownVote
        };

        return postBasicDto;
    }
}
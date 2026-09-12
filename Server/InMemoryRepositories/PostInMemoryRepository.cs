using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts = [];

    public PostInMemoryRepository()
    {
        CreateAndAddDummyData();
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? postToFind = posts.SingleOrDefault(p => p.Id == id);
        if (postToFind is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        return Task.FromResult(postToFind);
    }

    public IQueryable<Post> GetManyAsync()
    {
        return posts.AsQueryable();
    }

    private void CreateAndAddDummyData()
    {
        Post dummyPost = new()
        {
            CreatedAt = DateTime.UtcNow,
            Title = "Dummy Title",
            Body = "Dummy Body",
            UserId = 1
        };
        Post dummyPost2 = new()
        {
            CreatedAt = DateTime.UtcNow,
            Title = "Dummy Title2",
            Body = "Dummy Body2",
            UserId = 1
        };
        Post dummyPost3 = new()
        {
            CreatedAt = DateTime.UtcNow,
            Title = "Dummy Title3",
            Body = "Dummy Body3",
            UserId = 2
        };
        AddAsync(dummyPost);
        AddAsync(dummyPost2);
        AddAsync(dummyPost3);
        
    }
}
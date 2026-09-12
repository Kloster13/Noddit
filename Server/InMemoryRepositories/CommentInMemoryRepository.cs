using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments = [];

    public CommentInMemoryRepository()
    {
        CreateAndAddDummyData();
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any() ? comments.Max(c => c.Id) + 1 : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }

        comments.Remove(existingComment);
        comments.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? commentToFind = comments.SingleOrDefault(c => c.Id == id);
        if (commentToFind is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        return Task.FromResult(commentToFind);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return comments.AsQueryable();
    }

    private void CreateAndAddDummyData()
    {
        Comment dummyComment = new()
        {
            CreatedAt = DateTime.UtcNow,
            Text = "Dummy Text",
            UserId = 2,
            PostId = 1
        };
        Comment dummyComment2 = new()
        {
            CreatedAt = DateTime.UtcNow,
            Text = "Dummy Text2",
            UserId = 2,
            PostId = 1
        };
        Comment dummyComment3 = new()
        {
            CreatedAt = DateTime.UtcNow,
            Text = "Dummy Text",
            UserId = 1,
            PostId = 2
        };
        AddAsync(dummyComment);
        AddAsync(dummyComment2);
        AddAsync(dummyComment3);
    }
}
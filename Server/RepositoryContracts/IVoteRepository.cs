using Entities;

namespace RepositoryContracts;

public interface IVoteRepository
{
    Task<Vote> AddAsync(Vote vote);
    Task UpdateAsync(Vote vote);
    Task DeleteAsync(int id);
    Task<Vote> GetSingleAsync(int id);

    Task<Vote?> GetSingleByUserCommentPostAsync(int userId, int? postId,
        int? commentId);

    IQueryable<Vote> GetManyAsync();
}
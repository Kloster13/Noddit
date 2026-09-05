using Entities;

namespace RepositoryContracts;

public interface IVoteRepository
{
    Task<Vote> AddAsync(Vote vote);
    Task UpdateAsync(Vote vote);
    Task DeleteAsync(int id);
    Task<Vote> GetSingleAsync(int id);
    IQueryable<Vote> GetManyAsync();
}
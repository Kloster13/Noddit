using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class VoteRepository : IVoteRepository
{
    private List<Vote> votes = [];

    public VoteRepository()
    {
        CreateAndAddDummyData();
    }

    public Task<Vote> AddAsync(Vote vote)
    {
        vote.Id = votes.Any() ? votes.Max(v => v.Id) + 1 : 1;
        votes.Add(vote);
        return Task.FromResult(vote);
    }

    public Task UpdateAsync(Vote vote)
    {
        Vote? existingVote = votes.SingleOrDefault(v => v.Id == vote.Id);
        if (existingVote is null)
        {
            throw new InvalidOperationException(
                $"Vote with ID '{vote.Id}' not found");
        }

        votes.Remove(existingVote);
        votes.Add(vote);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Vote? voteToRemove = votes.SingleOrDefault(v => v.Id == id);
        if (voteToRemove is null)
        {
            throw new InvalidOperationException(
                $"Vote with ID '{id}' not found");
        }

        votes.Remove(voteToRemove);
        return Task.CompletedTask;
    }

    public Task<Vote> GetSingleAsync(int id)
    {
        Vote? voteToFind = votes.SingleOrDefault(v => v.Id == id);
        if (voteToFind is null)
        {
            throw new InvalidOperationException(
                $"Vote with ID '{id}' not found");
        }

        return Task.FromResult(voteToFind);
    }

    public IQueryable<Vote> GetManyAsync()
    {
        return votes.AsQueryable();
    }

    private void CreateAndAddDummyData()
    {
        Vote dummyVote = new()
        {
            UserId = 1,
            PostId = 1,
            Score = 1,
            CreatedAt = DateTime.UtcNow
        };
        votes.Add(dummyVote);
    }
}

using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories
{
    public class VoteFileRepository : IVoteRepository
    {
        private readonly string filePath = "data/votes.json";

        public VoteFileRepository()
        {
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "[]");
        }
        public async Task<Vote> AddAsync(Vote vote)
        {
            List<Vote> votes = await ReadVotesFromFile();
            int maxId = votes.Count > 0 ? votes.Max(v => v.Id) : 0;
            vote.Id = maxId + 1;
            votes.Add(vote);
            await WriteVotesToFile(votes);
            return vote;
        }

        public async Task DeleteAsync(int id)
        {
            List<Vote> votes = await ReadVotesFromFile();
            Vote? voteToRemove = votes.SingleOrDefault(v => v.Id == id) ?? throw new NotFoundException(
                    $"Vote with ID '{id}' not found");
            votes.Remove(voteToRemove);
            await WriteVotesToFile(votes);
        }

        public IQueryable<Vote> GetManyAsync()
        {
            List<Vote> votes = ReadVotesFromFile().Result;
            return votes.AsQueryable();
        }

        public async Task<Vote> GetSingleAsync(int id)
        {
            List<Vote> votes = await ReadVotesFromFile();
            return votes.SingleOrDefault(v => v.Id == id) ?? throw new NotFoundException($"Vote with ID '{id}' not found");
        }

        public async Task<Vote?> GetSingleByUserCommentPostAsync(int userId, int? postId, int? commentId)
        {
            if (postId is null == commentId is null)
                throw new ArgumentException("Exactly one of PostId or CommentId must be set");

            List<Vote> votes = await ReadVotesFromFile();
            if (postId is not null)
                return votes.SingleOrDefault(v => v.UserId == userId && v.PostId == postId);
            return votes.SingleOrDefault(v => v.UserId == userId && v.CommentId == commentId);
        }

        public async Task UpdateAsync(Vote vote)
        {
            List<Vote> votes = await ReadVotesFromFile();
            Vote? existingVote = votes.SingleOrDefault(v => v.Id == vote.Id) ?? throw new NotFoundException(
                    $"Vote with ID '{vote.Id}' not found");
            votes.Remove(existingVote);
            votes.Add(vote);
            await WriteVotesToFile(votes);
        }

        private async Task<List<Vote>> ReadVotesFromFile()
        {
            string votesAsJson = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Vote>>(votesAsJson)!;
        }

        private async Task WriteVotesToFile(List<Vote> votes)
        {
            string votesAsJson = JsonSerializer.Serialize(votes);
            await File.WriteAllTextAsync(filePath, votesAsJson);
        }
    }
}

using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories
{
    public class CommentFileRepository : ICommentRepository
    {
        private readonly string filePath = "data/comments.json";

        public CommentFileRepository()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "[]");
        }
        public async Task<Comment> AddAsync(Comment comment)
        {
            List<Comment> comments = await ReadCommentsFromFile();
            int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
            comment.Id = maxId + 1;
            comments.Add(comment);
            await WriteCommentsToFile(comments);
            return comment;
        }

        public async Task DeleteAsync(int id)
        {
            List<Comment> comments = await ReadCommentsFromFile();
            Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id) ?? throw new NotFoundException(
                    $"Comment with ID '{id}' not found");
            comments.Remove(commentToRemove);
            await WriteCommentsToFile(comments);
        }

        public IQueryable<Comment> GetManyAsync()
        {
            List<Comment> comments = ReadCommentsFromFile().Result;
            return comments.AsQueryable();
        }

        public async Task<Comment> GetSingleAsync(int id)
        {
            List<Comment> comments = await ReadCommentsFromFile();
            return comments.SingleOrDefault(c => c.Id == id) ?? throw new NotFoundException($"Comment with ID '{id}' not found");
        }

        public async Task UpdateAsync(Comment comment)
        {
            List<Comment> comments = await ReadCommentsFromFile();
            Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id) ?? throw new NotFoundException(
                    $"Comment with ID '{comment.Id}' not found");
            comments.Remove(existingComment);
            comments.Add(comment);
            await WriteCommentsToFile(comments);
        }

        private async Task<List<Comment>> ReadCommentsFromFile()
        {
            string commentsAsJson = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        }

        private async Task WriteCommentsToFile(List<Comment> comments)
        {
            string commentsAsJson = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(filePath, commentsAsJson);
        }
    }
}

using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories
{
    public class PostFileRepository : IPostRepository
    {
        private readonly string filePath = "data/posts.json";

        public PostFileRepository()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "[]");
        }
        public async Task<Post> AddAsync(Post post)
        {
            List<Post> posts = await ReadPostsFromFile();
            int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;
            post.Id = maxId + 1;
            posts.Add(post);
            await WritePostsToFile(posts);
            return post;
        }

        public async Task DeleteAsync(int id)
        {
            List<Post> posts = await ReadPostsFromFile();
            Post? postToRemove = posts.SingleOrDefault(p => p.Id == id) ?? throw new NotFoundException(
                    $"Post with ID '{id}' not found");
            posts.Remove(postToRemove);
            await WritePostsToFile(posts);
        }

        public IQueryable<Post> GetManyAsync()
        {
            List<Post> posts = ReadPostsFromFile().Result;
            return posts.AsQueryable();
        }

        public async Task<Post> GetSingleAsync(int id)
        {
            List<Post> posts = await ReadPostsFromFile();
            return posts.SingleOrDefault(p => p.Id == id) ?? throw new NotFoundException($"Post with ID '{id}' not found");
        }

        public async Task UpdateAsync(Post post)
        {
            List<Post> posts = await ReadPostsFromFile();
            Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id) ?? throw new NotFoundException(
                    $"Post with ID '{post.Id}' not found");
            posts.Remove(existingPost);
            posts.Add(post);
            await WritePostsToFile(posts);
        }

        private async Task<List<Post>> ReadPostsFromFile()
        {
            string postsAsJson = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        }

        private async Task WritePostsToFile(List<Post> posts)
        {
            string postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(filePath, postsAsJson);
        }
    }
}

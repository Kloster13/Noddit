namespace DTOs.Post;

public record CreatePostRequest(string? Title, string? Body, int UserId);
namespace DTOs.Comment;

public record CreateCommentRequest(int UserId, string? Text);
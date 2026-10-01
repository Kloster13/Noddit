namespace DTOs.Comment;

public record CommentResponseDto(int Id,string Text, int Votes, string CreatedBy);

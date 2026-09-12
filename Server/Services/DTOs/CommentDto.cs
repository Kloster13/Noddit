namespace Services.DTOs;

public record CommentDto(int Id,string Text, int Votes, string CreatedBy);
namespace Services.DTOs;

public record PostDto(int Id,int UserId,string Title, string Body, DateTime CreatedAt, int Votes);
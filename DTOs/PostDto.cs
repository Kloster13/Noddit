namespace DTOs;

public record PostDto(int Id,string CreatedBy,string Title, string Body, DateTime CreatedAt, int Votes);

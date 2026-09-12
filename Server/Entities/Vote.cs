namespace Entities;

public class Vote
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? PostId { get; set; }
    public int? CommentId { get; set; }
    public required int Score { get; set; } 
    public DateTime CreatedAt { get; set; }
}
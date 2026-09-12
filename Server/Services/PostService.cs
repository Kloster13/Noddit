using Entities;
using RepositoryContracts;
using Services.DTOs;

namespace Services;

public class PostService
{
    private readonly ICommentRepository commentRepo;
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly IVoteRepository voteRepo;

    public PostService(ICommentRepository commentRepo, IPostRepository postRepo, IUserRepository userRepo,
        IVoteRepository voteRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.voteRepo = voteRepo;
        this.commentRepo = commentRepo;
    }

    public List<PostDto> GetAllPosts() //TODO maybe add some form of filter to use same with users profile
    {
        var posts = postRepo.GetManyAsync();
        var resultList = new List<PostDto>();
        foreach (var post in posts)
        {
            var votes = CountVotes(post);
            var postDto = new PostDto(post.Id, post.UserId, post.Title, post.Body, post.CreatedAt, votes);
            resultList.Add(postDto);
        }

        return resultList;
    }

    public async Task<Post> GetSinglePost(int postId)
    {
        return await postRepo.GetSingleAsync(postId);
    }

    public async Task<PostDto> CreatePost(string? title, string? body, int userId)
    {
        if (title is null or "")
            throw new Exception("No title");
        if (body is null or "")
            throw new Exception("No body");

        var post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId,
            CreatedAt = DateTime.Now
        };
        var addedPost = await postRepo.AddAsync(post);
        Vote(userId, 1, addedPost.Id, null);
        var postDto = new PostDto(post.Id, addedPost.UserId, addedPost.Title, addedPost.Body,
            addedPost.CreatedAt, 1);
        return postDto;
    }

    private void Vote(int userId, int score, int? postId, int? commentId)
    {
        if ((postId is null) == (commentId is null))
            throw new ArgumentException("Exactly one of PostId or CommentId must be set");
        var vote = new Vote
        {
            UserId = userId,
            PostId = postId,
            CommentId = commentId,
            Score = score,
            CreatedAt = DateTime.Now
        };
        voteRepo.AddAsync(vote);
    }

    private int CountVotes(Post post)
    {
        return voteRepo.GetManyAsync().Where(v => v.PostId == post.Id).Sum(v => v.Score);
    }
}
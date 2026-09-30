using DTOs;
using Entities;
using RepositoryContracts;

namespace Services;

public class PostService
{
    private readonly ICommentRepository commentRepo;
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;
    private readonly IVoteRepository voteRepo;

    public PostService(ICommentRepository commentRepo, IPostRepository postRepo,
        IUserRepository userRepo,
        IVoteRepository voteRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
        this.voteRepo = voteRepo;
        this.commentRepo = commentRepo;
    }

    public List<PostDto>
        GetAllPosts() //TODO maybe add some form of filter to use same with users profile
    {
        var posts = postRepo.GetManyAsync();
        var resultList = new List<PostDto>();
        foreach (var post in posts)
        {
            var votes = CountPostVotes(post);
            var postDto = new PostDto(post.Id, GetCreatedBy(post.UserId), post.Title,
                post.Body, post.CreatedAt, votes);
            resultList.Add(postDto);
        }

        return resultList;
    }

    public async Task<Post> GetSinglePost(int postId)
    {
        return await postRepo.GetSingleAsync(postId);
    }

    public async Task<PostDto> CreatePost(string? title, string? body,
        int userId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title must be filled out");
        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Body must be filled out");

        var post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId,
            CreatedAt = DateTime.Now
        };
        var addedPost = await postRepo.AddAsync(post);
        await Vote(userId, 1, addedPost.Id, null);
        var postDto = new PostDto(post.Id,
            GetCreatedBy(userId),
            addedPost.Title,
            addedPost.Body,
            addedPost.CreatedAt, 1);
        return postDto;
    }

    public async Task<List<CommentResponseDto>> GetAllComments(int postId)
    {
        return
            commentRepo.GetManyAsync().Where(c => c.PostId == postId).Select(c =>
                new CommentResponseDto(c.Id, c.Text, CountCommentVotes(c), GetCreatedBy(c.UserId))).ToList();
    }

    public async Task Vote(int userId, int score, int? postId, int? commentId)
    {
        if (postId is null == commentId is null)
            throw new ArgumentException(
                "Exactly one of PostId or CommentId must be set");
        var vote =
            await voteRepo.GetSingleByUserCommentPostAsync(userId, postId,
                commentId);
        if (vote is null)
        {
            vote = new Vote
            {
                UserId = userId,
                PostId = postId,
                CommentId = commentId,
                Score = score,
                CreatedAt = DateTime.Now
            };
            await voteRepo.AddAsync(vote);
        }
        else
        {
            vote.Score = score;
            vote.CreatedAt = DateTime.Now;
            await voteRepo.UpdateAsync(vote);
        }
    }

    public async Task<Comment> CreateComment(int postId, int userId, string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text must be filled out");
        var commentToAdd = new Comment
        {
            UserId = userId,
            PostId = postId,
            Text = text,
            CreatedAt = DateTime.Now,
        };
        var addedComment = await commentRepo.AddAsync(commentToAdd);
        await Vote(userId, 1, null, addedComment.Id);
        return addedComment;
    }

    public async Task DeletePost(int postId)
    {
        var post = postRepo.GetSingleAsync(postId);
        List<Comment> commentsToDelete = commentRepo.GetManyAsync().Where(c => c.PostId == postId).ToList();
        foreach (var comment in commentsToDelete)
        {
            await commentRepo.DeleteAsync(comment.Id);
        }

        await postRepo.DeleteAsync(postId);
    }


    private string GetCreatedBy(int userId)
    {
        return userRepo.GetSingleAsync(userId).Result.Username;
    }

    private int CountPostVotes(Post post)
    {
        return voteRepo.GetManyAsync().Where(v => v.PostId == post.Id)
            .Sum(v => v.Score);
    }

    private int CountCommentVotes(Comment comment)
    {
        return voteRepo.GetManyAsync().Where(c => c.CommentId == comment.Id)
            .Sum(c => c.Score);
    }
}

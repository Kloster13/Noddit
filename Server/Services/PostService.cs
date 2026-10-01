using DTOs;
using DTOs.Comment;
using DTOs.Post;
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

    public async Task<PostDto> GetSinglePost(int postId)
    {
        var post = await postRepo.GetSingleAsync(postId);
        return new PostDto(post.Id, GetCreatedBy(post.UserId), post.Title,
            post.Body, post.CreatedAt, CountPostVotes(post));
    }

    public async Task<PostDto> CreatePost(CreatePostRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title must be filled out");
        if (string.IsNullOrWhiteSpace(request.Body))
            throw new ArgumentException("Body must be filled out");

        var post = new Post
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId,
            CreatedAt = DateTime.Now
        };
        var addedPost = await postRepo.AddAsync(post);
        await Vote(request.UserId, 1, addedPost.Id, null);
        var postDto = new PostDto(addedPost.Id,
            GetCreatedBy(request.UserId),
            addedPost.Title,
            addedPost.Body,
            addedPost.CreatedAt, 1);
        return postDto;
    }

    public async Task DeletePost(int postId)
    {
        await postRepo.GetSingleAsync(postId);
        var commentsToDelete = commentRepo.GetManyAsync().Where(c => c.PostId == postId).ToList();
        var votesToDelete = voteRepo.GetManyAsync()
            .Where(v => v.PostId == postId ||
                        (v.CommentId.HasValue && commentsToDelete.Any(c => c.Id == v.CommentId.Value)))
            .ToList();
        foreach (var vote in votesToDelete)
        {
            await voteRepo.DeleteAsync(vote.Id);
        }

        foreach (var comment in commentsToDelete)
        {
            await commentRepo.DeleteAsync(comment.Id);
        }

        await postRepo.DeleteAsync(postId);
    }

    public async Task<PostDto> UpdatePost(int postId, UpdatePostRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title must be filled out");
        if (string.IsNullOrWhiteSpace(request.Body))
            throw new ArgumentException("Body must be filled out");
        var postToUpdate = await postRepo.GetSingleAsync(postId);

        postToUpdate.Title = request.Title;
        postToUpdate.Body = request.Body;
        await postRepo.UpdateAsync(postToUpdate);
        var postDto = new PostDto(postToUpdate.Id,
            GetCreatedBy(postToUpdate.UserId),
            postToUpdate.Title,
            postToUpdate.Body,
            postToUpdate.CreatedAt, CountPostVotes(postToUpdate));
        return postDto;
    }

    public Task<List<CommentResponseDto>> GetAllCommentsOfPost(int postId)
    {
        return Task.FromResult(
            commentRepo.GetManyAsync().Where(c => c.PostId == postId).ToList().Select(c =>
                new CommentResponseDto(c.Id, c.Text, CountCommentVotes(c), GetCreatedBy(c.UserId))).ToList());
    }

    public async Task<CommentResponseDto> AddCommentToPost(int postId, CreateCommentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
            throw new ArgumentException("Text must be filled out");
        var commentToAdd = new Comment
        {
            UserId = request.UserId,
            PostId = postId,
            Text = request.Text,
            CreatedAt = DateTime.Now,
        };
        var addedComment = await commentRepo.AddAsync(commentToAdd);
        await Vote(request.UserId, 1, null, addedComment.Id);
        return new CommentResponseDto(addedComment.Id, addedComment.Text, 1,
            GetCreatedBy(addedComment.UserId));
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
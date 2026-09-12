using Entities;
using RepositoryContracts;
using Services.DTOs;

namespace Services;

public class UserService(
    IUserRepository userRepo,
    IVoteRepository voteRepo,
    IPostRepository postRepo,
    ICommentRepository commentRepo)
{
    public async Task<UserDto> CreateNewUser(string? username, string? password)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username must be filled out");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password must be filled out");
        var userToCreate = new User
        {
            Username = username,
            Password = password
        };
        var createdUser = await userRepo.AddAsync(userToCreate);
        return new UserDto(createdUser.Id, createdUser.Username, createdUser.Password, 0);
    }

    public List<User> GetAllUsers()
    {
        return userRepo.GetManyAsync().ToList();
    }

    private int CalculateUserKarma(int userId)
    {
        var postIds = postRepo.GetManyAsync().Where(p => p.UserId == userId).Select(p => p.Id).ToList();
        var commentIds = commentRepo.GetManyAsync().Where(c => c.UserId == userId).Select(c => c.Id).ToList();

        return voteRepo.GetManyAsync().Where(v =>
                (v.PostId.HasValue && postIds.Contains(v.PostId.Value)) ||
                (v.CommentId.HasValue && commentIds.Contains(v.CommentId.Value)))
            .Sum(v => v.Score);
    }
}
using DTOs;
using DTOs.User;
using Entities;
using RepositoryContracts;

namespace Services;

public class UserService(
    IUserRepository userRepo,
    IVoteRepository voteRepo,
    IPostRepository postRepo,
    ICommentRepository commentRepo)
{
    public async Task<UserDto> CreateNewUser(CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            throw new ArgumentException("Username must be filled out");
        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password must be filled out");
        if (userRepo.GetManyAsync().Any(u => u.Username == request.Username))
            throw new ArgumentException("Username is already taken");
        var userToCreate = new User
        {
            Username = request.Username,
            Password = request.Password,
        };
        var createdUser = await userRepo.AddAsync(userToCreate);
        return new UserDto(createdUser.Id, createdUser.Username,
            0);
    }

    public List<UserDto> GetAllUsers()
    {
        var users = userRepo.GetManyAsync().ToList();
        return users.Select(user => new UserDto(user.Id, user.Username, CalculateUserKarma(user.Id))).ToList();
    }

    public async Task<UserDto> GetSingleUser(int userId)
    {
      var userToGet = await userRepo.GetSingleAsync(userId);
      return new UserDto(userId, userToGet.Username, CalculateUserKarma(userId));
    }

    public async Task DeleteUser(int userId)
    {
      await userRepo.DeleteAsync(userId);
    }

    public async Task<UserDto> UpdateUser(int userId, UpdateUserRequest request)
    {
      if (string.IsNullOrWhiteSpace(request.Username))
            throw new ArgumentException("Username must be filled out");
      if (string.IsNullOrWhiteSpace(request.Password))
        throw new ArgumentException("Password must be filled out");
      
      var userToUpdate = await userRepo.GetSingleAsync(userId);
      userToUpdate.Username = request.Username;
      userToUpdate.Password = request.Password;
      await userRepo.UpdateAsync(userToUpdate);
      
      return new UserDto(userId, userToUpdate.Username, CalculateUserKarma(userId));
    }

    private int CalculateUserKarma(int userId)
    {
        var postIds = postRepo.GetManyAsync().Where(p => p.UserId == userId)
            .Select(p => p.Id).ToList();
        var commentIds = commentRepo.GetManyAsync()
            .Where(c => c.UserId == userId).Select(c => c.Id).ToList();

        return voteRepo.GetManyAsync().Where(v =>
                (v.PostId.HasValue && postIds.Contains(v.PostId.Value)) ||
                (v.CommentId.HasValue &&
                 commentIds.Contains(v.CommentId.Value)))
            .Sum(v => v.Score);
    }
}

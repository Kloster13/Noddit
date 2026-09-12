using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = [];

    public UserInMemoryRepository()
    {
        CreateAndAddDummyData();
    }

    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }

        users.Remove(existingUser);
        users.Add(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? userToFind = users.SingleOrDefault(u => u.Id == id);
        if (userToFind is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        return Task.FromResult(userToFind);
    }

    public IQueryable<User> GetManyAsync()
    {
        return users.AsQueryable();
    }

    private void CreateAndAddDummyData()
    {
        User dummyUser = new()
        {
            Username = "DummyUser",
            Password = "DummyPassword"
        };
        User dummyUser2 = new()
        {
            Username = "DummyUser2",
            Password = "DummyPassword2"
        };
        User dummyUser3 = new()
        {
            Username = "DummyUser3",
            Password = "DummyPassword3"
        };
        AddAsync(dummyUser);
        AddAsync(dummyUser2);
        AddAsync(dummyUser3);
        
    }
}

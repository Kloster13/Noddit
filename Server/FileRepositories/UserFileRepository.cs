using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace FileRepositories
{
    public class UserFileRepository : IUserRepository
    {
        private readonly string filePath = "data/users.json";

        public UserFileRepository()
        {
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "[]");
        }
        public async Task<User> AddAsync(User user)
        {
            List<User> users = await ReadUsersFromFile();
            int maxId = users.Count > 0 ? users.Max(u => u.Id) : 0;
            user.Id = maxId + 1;
            users.Add(user);
            await WriteUsersToFile(users);
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            List<User> users = await ReadUsersFromFile();
            User? userToRemove = users.SingleOrDefault(u => u.Id == id) ?? throw new InvalidOperationException(
                    $"User with ID '{id}' not found");
            users.Remove(userToRemove);
            await WriteUsersToFile(users);
        }

        public IQueryable<User> GetManyAsync()
        {
            List<User> users = ReadUsersFromFile().Result;
            return users.AsQueryable();
        }

        public async Task<User> GetSingleAsync(int id)
        {
            List<User> users = await ReadUsersFromFile();
            return users.SingleOrDefault(u => u.Id == id) ?? throw new InvalidOperationException($"User with ID '{id}' not found");
        }

        public async Task UpdateAsync(User user)
        {
            List<User> users = await ReadUsersFromFile();
            User? existingUser = users.SingleOrDefault(u => u.Id == user.Id) ?? throw new InvalidOperationException(
                    $"User with ID '{user.Id}' not found");
            users.Remove(existingUser);
            users.Add(user);
            await WriteUsersToFile(users);
        }

        private async Task<List<User>> ReadUsersFromFile()
        {
            string usersAsJson = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        }

        private async Task WriteUsersToFile(List<User> users)
        {
            string usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }
    }
}

using Entities;
using Services;

namespace CLI.UI.ManagePosts;

public class ManagePostsView(PostService postService, UserService userService)
{
    public async Task MainView()
    {
        while (true)
        {
            ViewPostOverview();
            var input = Console.ReadLine();
            if (int.TryParse(input, out var result))
            {
                try
                {
                    ViewSpecificPost(result);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                switch (input?.ToLower())
                {
                    case null:
                        Console.WriteLine("Please enter valid input you moron");
                        break;
                    case "post":
                        await CreatePostView();
                        break;
                    case "user":
                        await CreateNewUserView();
                        break;
                }
            }
        }
    }

    private void ViewPostOverview()
    {
        var posts = postService.GetAllPosts();
        foreach (var post in posts)
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"{post.Id} - {post.Title}  - votes: {post.Votes}");
        }

        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("Enter posts id to see fill post");
        Console.WriteLine("Other options: Create post (post) | create new user (user) ");
    }

    private async Task CreatePostView()
    {
        while (true)
        {
            Console.WriteLine("------------Create a post------------");
            Console.WriteLine("Enter Title");
            var titleInput = Console.ReadLine();
            Console.WriteLine("Enter Text");
            var textInput = Console.ReadLine();
            try
            {
                var post = await postService.CreatePost(titleInput, textInput, 1);
                Console.WriteLine($"New post created with title: {post.Title}");
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        await MainView();
    }

    private async Task CreateNewUserView()
    {
        while (true)
        {
            Console.WriteLine("------------Create new user ------------");
            Console.WriteLine("Enter Username");
            var inputUsername = Console.ReadLine();
            Console.WriteLine("Enter Password");
            var inputPassword = Console.ReadLine();
            try
            {
                var createdUser = await userService.CreateNewUser(inputUsername,inputPassword);
                Console.WriteLine($"User: {createdUser.Username} was created");
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private void ViewSpecificPost(int postId)
    {
        var post = postService.GetSinglePost(postId).Result;
        Console.WriteLine(post.Title);
        Console.WriteLine(post.Body);
        Console.WriteLine(post.CreatedAt);
    }
}
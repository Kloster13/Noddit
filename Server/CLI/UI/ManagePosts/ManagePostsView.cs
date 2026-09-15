using CLI.UI.ManageUsers;
using Entities;
using Services;

namespace CLI.UI.ManagePosts;

public class ManagePostsView(
    CliApp cliApp,
    PostService postService)
{
    public async Task MainView()
    {
        var runApp = true;
        while (runApp && cliApp.Running)
        {
            ViewPostOverview();
            var input = Console.ReadLine();
            if (int.TryParse(input, out var result))
            {
                try
                {
                    await ViewSpecificPost(result);
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
                    case "post":
                        await CreatePostView();
                        break;
                    case "user":
                        await GoToUserView();
                        break;
                    case "close":
                        cliApp.Close();
                        runApp = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input");
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
            Console.WriteLine(
                $"{post.Id} - {post.Title}  - votes: {post.Votes}");
        }

        Console.WriteLine("-------------------------------------------");
        Console.WriteLine("Enter posts id to see full post");
        Console.WriteLine(
            "Other options: Create post (post) | Go to User management (user), Close app (close)");
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
                var post =
                    await postService.CreatePost(titleInput, textInput, 1);
                Console.WriteLine($"New post created with title: {post.Title}");
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        await cliApp.SwitchToPostView();
    }

    private async Task GoToUserView()
    {
        await cliApp.SwitchToUserView();
    }

    private async Task ViewSpecificPost(int postId)
    {
        var singlePostView = new SinglePostView(this, postService, postId);
        await singlePostView.ShowSinglePostView();
    }
}
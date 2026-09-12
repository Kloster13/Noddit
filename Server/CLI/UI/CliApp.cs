using CLI.UI.ManagePosts;
using RepositoryContracts;
using Services;

namespace CLI.UI;

public class CliApp(
    PostService postService, UserService userService)
{
    public async Task StartAsync()
    {
        Console.WriteLine("Welcome to Noddit");
        var postsView = new ManagePostsView(postService,userService);
        await postsView.MainView();
    }
}
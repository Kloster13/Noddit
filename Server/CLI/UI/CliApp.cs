using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;
using Services;

namespace CLI.UI;

public class CliApp
{
    private readonly ManagePostsView postsView;
    private readonly ManageUsersView usersView;

    public bool Running { get; private set; } = true;

    public CliApp(UserService userService, PostService postService)
    {
        usersView = new ManageUsersView(this,userService);
        postsView = new ManagePostsView(this, postService, userService);
    }

    public void Close()
    {
        Running = false;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("Welcome to Noddit");
        await postsView.MainView();
    }

    public async Task SwitchToUserView()
    {
        await usersView.ShowUserView();
    }

    public async Task SwitchToPostView()
    {
        await postsView.MainView();
    }
}
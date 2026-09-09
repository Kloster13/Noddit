using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private IPostRepository postRepo;

    public ManagePostsView(IPostRepository postRepo)
    {
        this.postRepo = postRepo;
        Console.WriteLine("Hello there");
    }
    
}
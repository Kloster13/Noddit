using RepositoryContracts;

namespace CLI.UI;

public class CliApp(
    IUserRepository userRepository,
    ICommentRepository commentRepository,
    IVoteRepository voteRepository,
    IPostRepository postRepository)
{
    private IUserRepository userRepository = userRepository;
    private ICommentRepository commentRepository = commentRepository;
    private IVoteRepository voteRepository = voteRepository;
    private IPostRepository postRepository = postRepository;

    public async Task StartAsync()
    {
        throw new NotImplementedException();
    }
}
using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app..");
IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
IVoteRepository voteRepository = new VoteInMemoryRepository();

var cliApp = new CliApp(userRepository, commentRepository, voteRepository,
    postRepository);
    await cliApp.StartAsync();
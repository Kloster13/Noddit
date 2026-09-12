using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;
using Services;

Console.WriteLine("Starting CLI app..");
IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
IVoteRepository voteRepository = new VoteInMemoryRepository();

var postService = new PostService(commentRepository, postRepository, userRepository, voteRepository);
var userService = new UserService(userRepository,voteRepository,postRepository,commentRepository);

var cliApp = new CliApp(postService, userService);
await cliApp.StartAsync();
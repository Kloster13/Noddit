using CLI.UI;
using FileRepositories;
using RepositoryContracts;
using Services;

Console.WriteLine("Starting CLI app..");
IUserRepository userRepository = new UserFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();
IPostRepository postRepository = new PostFileRepository();
IVoteRepository voteRepository = new VoteFileRepository();

var postService = new PostService(commentRepository, postRepository, userRepository, voteRepository);
var userService = new UserService(userRepository,voteRepository,postRepository,commentRepository);

var cliApp = new CliApp(userService, postService);
await cliApp.StartAsync();
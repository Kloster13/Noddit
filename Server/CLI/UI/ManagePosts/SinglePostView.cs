using Entities;
using Services;

namespace CLI.UI.ManagePosts;

public class SinglePostView(
    ManagePostsView postsView,
    PostService postService,
    int postId)
{
    public async Task ShowSinglePostView()
    {
        var keepGoing = true;
        while (keepGoing)
        {
            var post = postService.GetSinglePost(postId).Result;
            Console.WriteLine($"--{post.Title}--");
            Console.WriteLine(post.Body);
            Console.WriteLine($"Created at: {post.CreatedAt}");
            await ShowComments();
            Console.WriteLine(
                "Options: Return to posts(back), Like post (like), dislike post (dislike), Comment on post (comment), Delete this post (delete)");
            var userInput = Console.ReadLine();
            switch (userInput?.ToLower())
            {
                case "back":
                    keepGoing = false;
                    break;
                case "like":
                    await VoteOnPost(1, 1);
                    break;
                case "dislike":
                    await VoteOnPost(1, -1);
                    break;
                case "comment":
                    await CreateNewComment(postId, 1);
                    break;
                case "delete":
                    await DeletePost(postId);
                    break;
                default:
                    Console.WriteLine("Command not valid");
                    break;
            }
        }

        await postsView.MainView();
    }

    private async Task ShowComments()
    {
        var comments = await postService.GetAllComments(postId);
        Console.WriteLine("------------------------------------------");
        foreach (var comment in comments)
        {
            Console.WriteLine($"{comment.Text} ");
            Console.WriteLine(
                $"created by: {comment.CreatedBy}, Karma: {comment.Votes}");
            Console.WriteLine(
                "----------------------------------------------------------");
        }
    }

    private async Task VoteOnPost(int userId, int score)
    {
        await postService.Vote(userId, score, postId, null);
    }

    private async Task CreateNewComment(int postId, int userId)
    {
        while (true)
        {
            Console.WriteLine("------------ New Comment ------------");
            Console.WriteLine("Enter Text");
            var textInput = Console.ReadLine();
            try
            {
                await postService.CreateComment(postId, userId, textInput);
                Console.WriteLine("Comment created succesfully");
                break;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private async Task DeletePost(int postId)
    {
        while (true)
        {
            Console.WriteLine("Are you sure you want to delete this post? (y) or (n)");
            var userInput = Console.ReadLine();
            if (userInput is not null && userInput.Equals("y"))
            {
                try
                {
                    await postService.DeletePost(postId);
                    Console.WriteLine("Post deleted succesfully!");
                    await postsView.MainView();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else break;
        }
    }
}

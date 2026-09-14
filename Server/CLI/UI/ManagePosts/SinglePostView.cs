using Entities;
using Services;
using Services.DTOs;

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
                "Options: Return to posts(back), Like post (like), dislike post (dislike)");
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
}
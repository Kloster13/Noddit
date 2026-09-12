using CLI.UI.ManagePosts;
using Services;

namespace CLI.UI.ManageUsers;

public class ManageUsersView(CliApp cliApp, UserService userService)
{
    public async Task ShowUserView()
    {
        while (cliApp.Running)
        {
            Console.WriteLine("------------ User management ------------");
            Console.WriteLine(
                "Options: See all users (1), Create New user (2), Go back to MainView (3)");
            var userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    ShowAllUsers();
                    break;
                case "2":
                    await CreateNewUserView();
                    break;
                case "3":
                    await ShowManagePostsView();
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    private async Task CreateNewUserView()
    {
        while (true)
        {
            Console.WriteLine("------------Create new user ------------");
            Console.WriteLine("Enter Username");
            var inputUsername = Console.ReadLine();
            Console.WriteLine("Enter Password");
            var inputPassword = Console.ReadLine();
            try
            {
                var createdUser =
                    await userService.CreateNewUser(inputUsername,
                        inputPassword);
                Console.WriteLine($"User: {createdUser.Username} was created");
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private void ShowAllUsers()
    {
        var userDtos = userService.GetAllUsers();
        Console.WriteLine("------------------- Users-----------------");
        foreach (var user in userDtos)
        {
            Console.WriteLine(
                $"{user.Id}| Username: {user.Username}, Karma: {user.Karma}");
        }
    }

    private async Task ShowManagePostsView()
    {
        await cliApp.SwitchToPostView();
    }
}
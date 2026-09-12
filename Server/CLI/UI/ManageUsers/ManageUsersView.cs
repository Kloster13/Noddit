using Services;

namespace CLI.UI.ManageUsers;

public class ManageUsersView(UserService userService)
{
    public async Task CreateNewUserView()
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
                var createdUser = await userService.CreateNewUser(inputUsername,inputPassword);
                Console.WriteLine($"User: {createdUser.Username} was created");
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private async Task ShowAllUsers()
    {
        var users = userService.GetAllUsers();
      
    }
}
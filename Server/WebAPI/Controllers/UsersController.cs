using DTOs;
using DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(UserService userService) : ControllerBase
{
  [HttpGet("{id:int}")]
  public async Task<IResult> GetUser([FromRoute] int id)
  {
    var toReturn = await userService.GetSingleUser(id);
    return Results.Ok(toReturn);
  }

  [HttpDelete("{id:int}")]
  public async Task<IResult> DeleteUser([FromRoute] int id)
  {
    await userService.DeleteUser(id);
    return Results.Ok();
  }

  [HttpPost]
  public async Task<IResult> CreateUser([FromBody] CreateUserRequest request)
  {
    var toReturn = await userService.CreateNewUser(request);
    return Results.Created($"/api/users/{toReturn.Id}", toReturn);
  }

  [HttpPut("{id:int}")]
  public async Task<IResult> UpdateUser([FromRoute] int id, [FromBody] UpdateUserRequest request)
  {
    var toUpdate= await userService.UpdateUser(id, request);
    return Results.Ok(toUpdate);
  }
}

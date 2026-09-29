using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController(PostService postService) : ControllerBase
{
  [HttpGet("{id:int}")]
  public async Task<IResult> GetPost([FromRoute] int id)
  {
    var toReturn = await postService.GetSinglePost(id);
    return Results.Ok(toReturn);
  }
}

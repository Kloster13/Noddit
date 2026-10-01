using DTOs;
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

  [HttpGet]
  public async Task<ActionResult> GetPosts()
  {
    var toReturn = postService.GetAllPosts();
    return Ok(toReturn);
  }

  [HttpPost]
  public async Task<IResult> CreatePost(CreatePostRequest request)
  {
    var toReturn = await postService.CreatePost(request);
    return Results.Ok(toReturn);
  }

  [HttpPut("{id:int}")]
  public async Task<IResult> UpdatePost([FromRoute]int id, UpdatePostRequest request)
  {
    var toReturn = await postService.UpdatePost(id, request);
    return Results.Ok(toReturn);
  }
}

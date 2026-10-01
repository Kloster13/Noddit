using DTOs;
using DTOs.Comment;
using DTOs.Post;
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
    public ActionResult<List<PostDto>> GetPosts()
    {
        var toReturn = postService.GetAllPosts();
        return Ok(toReturn);
    }

    [HttpPost]
    public async Task<IResult> CreatePost(CreatePostRequest request)
    {
        var toReturn = await postService.CreatePost(request);
        return Results.Created($"/api/posts/{toReturn.Id}", toReturn);
    }

    [HttpPut("{id:int}")]
    public async Task<IResult> UpdatePost([FromRoute] int id,
        UpdatePostRequest request)
    {
        var toReturn = await postService.UpdatePost(id, request);
        return Results.Ok(toReturn);
    }

    [HttpDelete("{id:int}")]
    public async Task<IResult> DeletePost([FromRoute] int id)
    {
        await postService.DeletePost(id);
        return Results.Ok();
    }

    [HttpPost("{id:int}/comments")]
    public async Task<IResult> AddCommentToPost([FromRoute] int id,
        CreateCommentRequest request)
    {
        var toReturn = await postService.AddCommentToPost(id, request);
        return Results.Created($"/api/posts/{id}/comments", toReturn);
    }

    [HttpGet("{id:int}/comments")]
    public async Task<IResult> GetCommentsOfPost([FromRoute] int id)
    {
        var toReturn = await postService.GetAllCommentsOfPost(id);
        return Results.Ok(toReturn);
    }
}
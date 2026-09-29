using RepositoryContracts;

namespace WebAPI;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await next(context);
    }
    catch (ArgumentException e)
    {
      context.Response.StatusCode = 400;
      await context.Response.WriteAsJsonAsync(new { error = e.Message });
    }
    catch (NotFoundException e)
    {
      context.Response.StatusCode = 404;
      await context.Response.WriteAsJsonAsync(new { error = e.Message });
    }
    catch (InvalidOperationException e)
    {
      context.Response.StatusCode = 500;
      await context.Response.WriteAsJsonAsync(new { error = e.Message });
    }
    catch (Exception)
    {
      context.Response.StatusCode = 500;
      await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred" });
    }
  }
}

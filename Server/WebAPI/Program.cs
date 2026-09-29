using FileRepositories;
using RepositoryContracts;
using Services;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserFileRepository>();
builder.Services.AddScoped<IPostRepository, PostFileRepository>();
builder.Services.AddScoped<IVoteRepository, VoteFileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();


// Configure the HTTP request pipeline. TODO
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
// app.UseHttpsRedirection();
app.MapControllers();
app.Run();

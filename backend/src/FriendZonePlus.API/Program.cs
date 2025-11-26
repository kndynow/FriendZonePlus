using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Infrastructure.Repositories;
using FriendZonePlus.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using FriendZonePlus.API.Endpoints;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=friendzoneplus.db";

//Config for EF Core with SQLite
builder.Services.AddDbContext<FriendZonePlusContext>(options => options.UseSqlite(connectionString));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PostService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapUserEnpoints();
app.MapPostEndpoints();

// Create or update database on every run
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FriendZonePlusContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured while migrating the database");
    }
}

app.Run();

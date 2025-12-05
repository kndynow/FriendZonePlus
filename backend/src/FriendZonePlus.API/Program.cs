using FluentValidation;
using FriendZonePlus.API.Endpoints;
using FriendZonePlus.API.Mappings;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Helpers.PasswordHelpers;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Application.Services.Messages;
using FriendZonePlus.Application.Validators;
using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Infrastructure.Data;
using FriendZonePlus.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=friendzoneplus.db";

//Config for EF Core with SQLite
builder.Services.AddDbContext<FriendZonePlusContext>(options => options.UseSqlite(connectionString));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWallPostRepository, WallPostRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WallPostService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<FollowService>();
builder.Services.AddScoped<IMessageService, MessageService>();

// Helper
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
//Validator
builder.Services.AddScoped<IFollowValidator, FollowValidator>();
builder.Services.AddScoped<IValidator<RegisterUserRequestDto>, RegisterUserRequestDtoValidator>();
builder.Services.AddScoped<IValidator<SendMessageRequestDto>, SendMessageRequestDtoValidator>();

//Mapster Configuration
var config = TypeAdapterConfig.GlobalSettings;
FollowMappings.ConfigureFollowMappings();
WallPostMappings.ConfigureWallPostMappings();
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, Mapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapAuthEndpoints();
app.MapWallPostEndpoints();
app.MapFollowEndpoints();
app.MapUserEndpoints();
app.MapMessageEndpoints();

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

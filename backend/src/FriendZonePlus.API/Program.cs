using FluentValidation;
using FriendZonePlus.API.Endpoints;
using FriendZonePlus.API.Mappings;
using FriendZonePlus.API.Validators;
using FriendZonePlus.Application.Helpers.PasswordHelpers;
using FriendZonePlus.Application.Services;
using FriendZonePlus.API.DTOs;
using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Infrastructure.Data;
using FriendZonePlus.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using FriendZonePlus.Application.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FriendZonePlus.Application.Helpers;
using FriendZonePlus.Application.Helpers.JwtHelper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=friendzoneplus.db";

//Config for EF Core with SQLite
builder.Services.AddDbContext<FriendZonePlusContext>(options => options.UseSqlite(connectionString));

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

builder.Services.AddAuthorization();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWallPostRepository, WallPostRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WallPostService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<FollowService>();
builder.Services.AddScoped<IFollowValidator, FollowValidator>();
// Helper
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
//Validator
builder.Services.AddScoped<IValidator<RegisterUserRequestDto>, RegisterUserRequestDtoValidator>();


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

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapWallPostEndpoints();
app.MapFollowEndpoints();
app.MapUserEndpoints();

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

public partial class Program { }
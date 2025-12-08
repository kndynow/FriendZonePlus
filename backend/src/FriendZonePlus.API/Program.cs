using FluentValidation;
using FriendZonePlus.API.Endpoints;
using FriendZonePlus.API.Hubs;
using FriendZonePlus.API.Mappings;
using FriendZonePlus.Application.Validators;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Helpers.PasswordHelpers;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Application.Services.Messages;
using FriendZonePlus.Application.Interfaces;
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
using System.Text.Json;
using FriendZonePlus.Infrastructure.Authentication;
using FriendZonePlus.API.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Configure JSON serialization to use camelCase
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=friendzoneplus.db";

// Exception handling
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//Config for EF Core with SQLite
builder.Services.AddDbContext<FriendZonePlusContext>(options => options.UseSqlite(connectionString));

// JWT Configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
builder.Services.AddSingleton<IJwtGenerator, JwtTokenGenerator>();
builder.Services.AddSingleton<ITokenValidator, TokenValidator>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings?.Issuer,
            ValidAudience = jwtSettings?.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings?.SecretKey ?? throw new InvalidOperationException("JWT SecretKey is not configured"))
            )
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["auth"];
                return Task.CompletedTask;
            }
        };

    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWallPostRepository, WallPostRepository>();
// builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

// Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WallPostService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<FollowService>();
builder.Services.AddScoped<IMessageService, MessageService>();

// Helper
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
//Validator
builder.Services.AddScoped<IFollowValidator, FollowValidator>();
builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
builder.Services.AddScoped<IValidator<SendMessageRequestDto>, SendMessageRequestDtoValidator>();

//SignalR
builder.Services.AddSignalR();

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
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapWallPostEndpoints();
app.MapFollowEndpoints();
app.MapUserEndpoints();
app.MapMessageEndpoints();

app.MapHub<MessageHub>("/hubs/message");

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
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

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

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

app.Run();

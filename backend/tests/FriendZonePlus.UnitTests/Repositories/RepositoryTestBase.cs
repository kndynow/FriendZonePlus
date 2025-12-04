using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using FriendZonePlus.Infrastructure.Data;

namespace FriendZonePlus.UnitTests.Repositories;

public class RepositoryTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<FriendZonePlusContext> _options;

    public RepositoryTestBase()
    {
        // Create in-memory database
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        // Configure context to use in-memory database
        _options = new DbContextOptionsBuilder<FriendZonePlusContext>()
        .UseSqlite(_connection)
        .Options;

        // Create db schema
        using var context = new FriendZonePlusContext(_options);
        context.Database.EnsureCreated();
    }

    // Helper method to create a new clean context
    protected FriendZonePlusContext CreateContext()
    {
        return new FriendZonePlusContext(_options);
    }

    // Dispose method to clean up memory when closed
    public void Dispose()
    {
        _connection.Dispose();
    }
}

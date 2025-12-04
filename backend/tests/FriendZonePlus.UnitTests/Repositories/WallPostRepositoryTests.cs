using FriendZonePlus.Core.Entities;
using FriendZonePlus.Infrastructure.Data;
using FriendZonePlus.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FriendZonePlus.UnitTests.Repositories;

public class WallPostRepositoryTests : RepositoryTestBase
{
    [Fact]
    public async Task AddWallPost_ShouldPersistData()
    {
        //Arrange
        using var context = CreateContext();
        await SeedUsers(context);
        var repository = new WallPostRepository(context);
        var newWallPost = new WallPost
        {
            Content = "Test post",
            AuthorId = 1,
            TargetUserId = 2,
            CreatedAt = DateTime.UtcNow
        };

        //Act
        await repository.AddAsync(newWallPost);

        //Assert
        using var verifyContext = CreateContext();
        var savedPost = await verifyContext.WallPosts.FirstOrDefaultAsync();

        Assert.NotNull(savedPost);
        Assert.Equal("Test post", savedPost.Content);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturn_CorrectPost()
    {
        //Arrange
        int wallPostId;
        using (var seedContext = CreateContext())
        {
            await SeedUsers(seedContext);
            wallPostId = await SeedWallPosts(seedContext);
        }

        //Act
        using var context = CreateContext();
        var repository = new WallPostRepository(context);
        var result = await repository.GetByIdAsync(wallPostId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Test post", result.Content);
    }

    [Fact]
    public async Task GetByTargetUserIdAsync_ShouldReturn_CorrectPosts()
    {
        //Arrange
        using (var seedContext = CreateContext())
        {
            await SeedUsers(seedContext);
            await SeedWallPosts(seedContext);
        }

        //Act
        using var context = CreateContext();
        var repository = new WallPostRepository(context);
        var result = await repository.GetByTargetUserIdAsync(2);

        //Assert
        Assert.NotNull(result);
        Assert.Single(result.ToList());
        Assert.Equal("Test post", result.First().Content);
    }

    [Fact]
    public async Task GetByTargetUserIdAsync_ShouldReturn_EmptyList_WhenNoPosts()
    {
        //Arrange
        using (var seedContext = CreateContext())
        {
            await SeedUsers(seedContext);
        }

        //Act
        using var context = CreateContext();
        var repository = new WallPostRepository(context);
        var result = await repository.GetByTargetUserIdAsync(2);

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.Empty(result.ToList());
    }



    // Helper methods for seeding data
    private async Task SeedUsers(FriendZonePlusContext context)
    {
        context.Users.AddRange(
            new User { Id = 1, Username = "User1", Email = "user1@example.com" },
            new User { Id = 2, Username = "User2", Email = "user2@example.com" }
        );
        await context.SaveChangesAsync();
    }

    private async Task<int> SeedWallPosts(FriendZonePlusContext context)
    {
        var post = new WallPost { Content = "Test post", AuthorId = 1, TargetUserId = 2, CreatedAt = DateTime.UtcNow };
        context.WallPosts.Add(post);
        await context.SaveChangesAsync();
        return post.Id;
    }
}
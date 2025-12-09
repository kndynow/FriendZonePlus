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
    public async Task GetWallPostsAsync_ShouldReturn_CorrectPosts()
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
        var result = await repository.GetWallPostsAsync(2);

        //Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test post", result.First().Content);
        Assert.NotNull(result.First().Author);
        Assert.Equal("User1", result.First().Author.Username);
    }

    [Fact]
    public async Task GetWallPostsAsync_ShouldReturn_EmptyList_WhenNoPosts()
    {
        //Arrange
        using (var seedContext = CreateContext())
        {
            await SeedUsers(seedContext);
        }

        //Act
        using var context = CreateContext();
        var repository = new WallPostRepository(context);
        var result = await repository.GetWallPostsAsync(2);

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_ExistingPost()
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
        var post = await repository.GetByIdAsync(wallPostId);
        Assert.NotNull(post);

        post.Content = "Updated post content";
        await repository.UpdateAsync(post);

        //Assert
        using var verifyContext = CreateContext();
        var updatedPost = await verifyContext.WallPosts.FindAsync(wallPostId);
        Assert.NotNull(updatedPost);
        Assert.Equal("Updated post content", updatedPost.Content);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemove_Post()
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
        var post = await repository.GetByIdAsync(wallPostId);
        Assert.NotNull(post);

        await repository.DeleteAsync(post);

        //Assert
        using var verifyContext = CreateContext();
        var deletedPost = await verifyContext.WallPosts.FindAsync(wallPostId);
        Assert.Null(deletedPost);
    }

    [Fact]
    public async Task GetFeedAsync_ShouldReturn_PostsFromFollowedUsers()
    {
        //Arrange
        using (var seedContext = CreateContext())
        {
            await SeedUsers(seedContext);
            await SeedFollows(seedContext);
            // Only seed posts from User2 (who User1 follows)
            seedContext.WallPosts.AddRange(
                new WallPost { Content = "Post 1 from User2", AuthorId = 2, TargetUserId = 2, CreatedAt = DateTime.UtcNow.AddMinutes(-10) },
                new WallPost { Content = "Post 2 from User2", AuthorId = 2, TargetUserId = 2, CreatedAt = DateTime.UtcNow.AddMinutes(-5) }
            );
            await seedContext.SaveChangesAsync();
        }

        //Act
        using var context = CreateContext();
        var repository = new WallPostRepository(context);
        var result = await repository.GetFeedAsync(1); // User1 follows User2

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, post =>
        {
            Assert.NotNull(post.Author);
            Assert.Equal(2, post.AuthorId); // All posts should be from User2
        });
        // Verify ordering (newest first)
        Assert.True(result[0].CreatedAt >= result[1].CreatedAt);
    }

    [Fact]
    public async Task GetFeedAsync_ShouldReturn_EmptyList_WhenNoFollows()
    {
        //Arrange
        using (var seedContext = CreateContext())
        {
            await SeedUsers(seedContext);
            // Only seed posts from User2 (User1 doesn't follow anyone and has no own posts)
            seedContext.WallPosts.AddRange(
                new WallPost { Content = "Post 1 from User2", AuthorId = 2, TargetUserId = 2, CreatedAt = DateTime.UtcNow.AddMinutes(-10) },
                new WallPost { Content = "Post 2 from User2", AuthorId = 2, TargetUserId = 2, CreatedAt = DateTime.UtcNow.AddMinutes(-5) }
            );
            await seedContext.SaveChangesAsync();
        }

        //Act
        using var context = CreateContext();
        var repository = new WallPostRepository(context);
        var result = await repository.GetFeedAsync(1); // User1 doesn't follow anyone

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result);
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

    private async Task SeedFollows(FriendZonePlusContext context)
    {
        context.Follows.Add(new Follow
        {
            FollowerId = 1,
            FollowedUserId = 2,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();
    }

    private async Task SeedWallPostsForFeed(FriendZonePlusContext context)
    {
        // Create posts from User2 (who User1 follows)
        context.WallPosts.AddRange(
            new WallPost { Content = "Post 1 from User2", AuthorId = 2, TargetUserId = 2, CreatedAt = DateTime.UtcNow.AddMinutes(-10) },
            new WallPost { Content = "Post 2 from User2", AuthorId = 2, TargetUserId = 2, CreatedAt = DateTime.UtcNow.AddMinutes(-5) }
        );
        // Create a post from User1 (who User1 doesn't follow themselves in feed context)
        context.WallPosts.Add(
            new WallPost { Content = "Post from User1", AuthorId = 1, TargetUserId = 1, CreatedAt = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();
    }
}
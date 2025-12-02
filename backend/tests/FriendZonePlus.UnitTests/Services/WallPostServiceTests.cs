using System;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;

namespace FriendZonePlus.UnitTests.Services;

public class WallPostServiceTests
{
  #region Setup
  private readonly Mock<IWallPostRepository> _WallPostRepoMock;
  private readonly Mock<IUserRepository> _userRepoMock;
  private readonly Mock<IFollowRepository> _followRepoMock;
  private readonly WallPostService _sut;

  public WallPostServiceTests()
  {
    _WallPostRepoMock = new Mock<IWallPostRepository>();
    _userRepoMock = new Mock<IUserRepository>();
    _followRepoMock = new Mock<IFollowRepository>();
    _sut = new WallPostService(_WallPostRepoMock.Object, _userRepoMock.Object, _followRepoMock.Object);
  }

  #endregion

  #region CreateWallPostAsync


  [Fact]
  public async Task CreateWallPost_ShouldReturnWallPost_WhenDataIsValid()
  {
    // Arrange
    var dto = new CreateWallPostDto(1, 2, "This is a post!");

    // Setup Users
    _userRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new User());

    // Simulate that database returns an ID
    _WallPostRepoMock.Setup(x => x.AddAsync(It.IsAny<WallPost>())).ReturnsAsync((WallPost p) =>
    {
      p.Id = 101;
      return p;
    });

    // Act
    var result = await _sut.CreateWallPostAsync(dto);

    // Assert
    Assert.Equal(101, result.Id);
    Assert.Equal(dto.Content, result.Content);
    Assert.Equal(dto.AuthorId, result.AuthorId);
    Assert.True(result.CreatedAt > DateTime.MinValue);

    _WallPostRepoMock.Verify(x => x.AddAsync(It.IsAny<WallPost>()), Times.Once);

  }

  [Theory]
  [InlineData("")]
  [InlineData("     ")]
  public async Task CreateWallPost_ShouldThrowException_WhenContentIsInvalid(string invalidContent)
  {
    // Arrange & Act
    var dto = new CreateWallPostDto(1, 2, invalidContent);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateWallPostAsync(dto));
    Assert.Equal("Content cannot be empty", ex.Message);
  }

  [Fact]
  public async Task CreateWallPost_ShouldThrowException_WhenContentIsTooLong()
  {
    // Arrange
    var longContent = new string('x', 301);
    var dto = new CreateWallPostDto(1, 2, longContent);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateWallPostAsync(dto));
    Assert.Equal("Content too long", ex.Message);
  }

  [Fact]
  public async Task CreateWallPost_ShouldThrowException_WhenAuthorDoesNotExist()
  {
    //Arrange
    var dto = new CreateWallPostDto(99, 2, "This is a post!");

    //Mock so that UserRepo returns null when we query for ID 99
    _userRepoMock.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((User?)null);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateWallPostAsync(dto));
    Assert.Equal("Author does not exist", ex.Message);
  }

  [Fact]
  public async Task CreateWallPost_ShouldThrowException_WhenTargetUserDoesNotExist()
  {
    // Arrange
    var dto = new CreateWallPostDto(1, 99, "This is a post!");

    // Author exists, target does not
    _userRepoMock.Setup(x => x.GetByIdAsync(dto.AuthorId)).ReturnsAsync(new User { Id = dto.AuthorId });
    _userRepoMock.Setup(x => x.GetByIdAsync(dto.TargetUserId)).ReturnsAsync((User?)null);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateWallPostAsync(dto));
    Assert.Equal("Target user does not exist", ex.Message);
  }

  #endregion

  #region GetFeedForUserAsync

  [Fact]
  public async Task GetFeedForUserAsync_ShouldThrowException_WhenUserDoesNotExist()
  {
    // Arrange
    var userId = 99;
    _userRepoMock.Setup(x => x.GetByIdAsync(userId))
                 .ReturnsAsync((User?)null);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetFeedForUserAsync(userId));
    Assert.Equal("User does not exist", ex.Message);
  }

  [Fact]
  public async Task GetFeedForUserAsync_ShouldReturnEmptyList_WhenUserFollowsButNoPosts()
  {
    // Arrange
    var userId = 1;
    _userRepoMock.Setup(x => x.GetByIdAsync(userId))
                 .ReturnsAsync(new User { Id = userId });

    _followRepoMock.Setup(x => x.GetFollowedUserIdsAsync(userId))
                   .ReturnsAsync(new List<int> { 2, 3 });

    //User follows id 2, 3
    var followedUserIds = new List<int> { 2, 3 };

    //No posts for followed users
    _WallPostRepoMock.Setup(x => x.GetFeedForUserAsync(
      It.Is<IEnumerable<int>>(ids => ids.SequenceEqual(followedUserIds))))
                     .ReturnsAsync(new List<WallPost>());

    // Act
    var result = await _sut.GetFeedForUserAsync(userId);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
  }

  [Fact]
  public async Task GetFeedForUserAsync_ShouldReturnMappedPosts_WhenPostsExist()
  {
    // Arrange
    var userId = 1;

    _userRepoMock.Setup(x => x.GetByIdAsync(userId))
                 .ReturnsAsync(new User { Id = userId });

    var followedUserIds = new List<int> { 2, 3 };

    _followRepoMock.Setup(x => x.GetFollowedUserIdsAsync(userId))
                   .ReturnsAsync(followedUserIds);

    var now = DateTime.UtcNow;
    var posts = new List<WallPost>
    {
        new WallPost { Id = 1, Content = "Post 1", AuthorId = 2, TargetUserId = 1, CreatedAt = now.AddMinutes(-5) },
        new WallPost { Id = 2, Content = "Post 2", AuthorId = 3, TargetUserId = 1, CreatedAt = now }
    };

    _WallPostRepoMock.Setup(x => x.GetFeedForUserAsync(
            It.Is<IEnumerable<int>>(ids => ids.SequenceEqual(followedUserIds))))
                     .ReturnsAsync(posts);

    // Act
    var result = (await _sut.GetFeedForUserAsync(userId)).ToList();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal(posts[0].Id, result[0].Id);
    Assert.Equal(posts[0].Content, result[0].Content);
    Assert.Equal(posts[1].Id, result[1].Id);
    Assert.Equal(posts[1].Content, result[1].Content);
  }

  #endregion

  #region GetWallPostsForAuthorAsync

  [Fact]
  public async Task GetWallPostsForAuthorAsync_ShouldThrowException_WhenAuthorDoesNotExist()
  {
    // Arrange
    var authorId = 5;
    _userRepoMock.Setup(x => x.GetByIdAsync(authorId))
                 .ReturnsAsync((User?)null);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetWallPostsForAuthorAsync(authorId));
    Assert.Equal("Author does not exist", ex.Message);
  }

  [Fact]
  public async Task GetWallPostsForAuthorAsync_ShouldReturnMappedPosts_WhenPostsExist()
  {
    // Arrange
    var authorId = 5;
    _userRepoMock.Setup(x => x.GetByIdAsync(authorId))
                 .ReturnsAsync(new User { Id = authorId });

    var now = DateTime.UtcNow;
    var posts = new List<WallPost>
    {
      new WallPost { Id = 10, Content = "Author post 1", AuthorId = authorId, TargetUserId = 1, CreatedAt = now.AddMinutes(-10) },
      new WallPost { Id = 11, Content = "Author post 2", AuthorId = authorId, TargetUserId = 2, CreatedAt = now }
    };

    _WallPostRepoMock.Setup(x => x.GetByAuthorIdAsync(authorId))
                     .ReturnsAsync(posts);

    // Act
    var result = (await _sut.GetWallPostsForAuthorAsync(authorId)).ToList();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal(posts[0].Id, result[0].Id);
    Assert.Equal(posts[0].Content, result[0].Content);
    Assert.Equal(posts[1].Id, result[1].Id);
    Assert.Equal(posts[1].Content, result[1].Content);
  }

  #endregion

  #region GetWallPostsForTargetUserAsync

  [Fact]
  public async Task GetWallPostsForTargetUserAsync_ShouldThrowException_WhenTargetUserDoesNotExist()
  {
    // Arrange
    var targetUserId = 7;
    _userRepoMock.Setup(x => x.GetByIdAsync(targetUserId))
                 .ReturnsAsync((User?)null);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetWallPostsForTargetUserAsync(targetUserId));
    Assert.Equal("Target user does not exist", ex.Message);
  }

  [Fact]
  public async Task GetWallPostsForTargetUserAsync_ShouldReturnMappedPosts_WhenPostsExist()
  {
    // Arrange
    var targetUserId = 7;
    _userRepoMock.Setup(x => x.GetByIdAsync(targetUserId))
                 .ReturnsAsync(new User { Id = targetUserId });

    var now = DateTime.UtcNow;
    var posts = new List<WallPost>
    {
      new WallPost { Id = 20, Content = "Target post 1", AuthorId = 1, TargetUserId = targetUserId, CreatedAt = now.AddMinutes(-3) },
      new WallPost { Id = 21, Content = "Target post 2", AuthorId = 2, TargetUserId = targetUserId, CreatedAt = now }
    };

    _WallPostRepoMock.Setup(x => x.GetByTargetUserIdAsync(targetUserId))
                     .ReturnsAsync(posts);

    // Act
    var result = (await _sut.GetWallPostsForTargetUserAsync(targetUserId)).ToList();

    // Assert
    Assert.Equal(2, result.Count);
    Assert.Equal(posts[0].Id, result[0].Id);
    Assert.Equal(posts[0].Content, result[0].Content);
    Assert.Equal(posts[1].Id, result[1].Id);
    Assert.Equal(posts[1].Content, result[1].Content);
  }

  #endregion

  #region UpdateWallPostAsync

  [Fact]
  public async Task UpdateWallPostAsync_ShouldReturnUpdatedWallPost_WhenDataIsValid()
  {
    // Arrange
    var wallPostId = 1;
    var authorId = 5;
    var updatedContent = "Updated content";
    var originalCreatedAt = DateTime.UtcNow.AddDays(-1);

    var dto = new UpdateWallPostDto(wallPostId, updatedContent, authorId, DateTime.UtcNow);

    var existingWallPost = new WallPost
    {
      Id = wallPostId,
      AuthorId = authorId,
      Content = "Original content",
      TargetUserId = 10,
      CreatedAt = originalCreatedAt
    };

    var updatedWallPost = new WallPost
    {
      Id = wallPostId,
      AuthorId = authorId,
      Content = updatedContent,
      TargetUserId = 10,
      CreatedAt = originalCreatedAt
    };

    _WallPostRepoMock.Setup(x => x.GetByIdAsync(wallPostId))
                     .ReturnsAsync(existingWallPost);

    _WallPostRepoMock.Setup(x => x.UpdateAsync(It.IsAny<WallPost>()))
                     .ReturnsAsync(updatedWallPost);

    // Act
    var result = await _sut.UpdateWallPostAsync(dto);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(wallPostId, result.Id);
    Assert.Equal(authorId, result.AuthorId);
    Assert.Equal(updatedContent, result.Content);
    Assert.Equal(originalCreatedAt, result.CreatedAt);

    _WallPostRepoMock.Verify(x => x.GetByIdAsync(wallPostId), Times.Once);
    _WallPostRepoMock.Verify(x => x.UpdateAsync(It.Is<WallPost>(wp =>
      wp.Id == wallPostId &&
      wp.Content == updatedContent &&
      wp.AuthorId == authorId)), Times.Once);
  }

  #endregion

  #region DeleteWallPostAsync

  [Fact]
  public async Task DeleteWallPostAsync_ShouldDeleteWallPost_WhenWallPostExists()
  {
    // Arrange
    var wallPostId = 1;
    var authorId = 5;

    var existingWallPost = new WallPost
    {
      Id = wallPostId,
      AuthorId = authorId,
      Content = "Content to delete",
      TargetUserId = 10,
      CreatedAt = DateTime.UtcNow.AddDays(-1)
    };

    _WallPostRepoMock.Setup(x => x.GetByIdAsync(wallPostId))
                     .ReturnsAsync(existingWallPost);

    _WallPostRepoMock.Setup(x => x.DeleteAsync(wallPostId))
                     .Returns(Task.CompletedTask);

    // Act
    await _sut.DeleteWallPostAsync(wallPostId);

    // Assert
    _WallPostRepoMock.Verify(x => x.GetByIdAsync(wallPostId), Times.Once);
    _WallPostRepoMock.Verify(x => x.DeleteAsync(wallPostId), Times.Once);
  }

  #endregion


}

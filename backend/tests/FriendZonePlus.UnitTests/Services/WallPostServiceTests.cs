using System;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Interfaces;
using FriendZonePlus.Application.Mappings;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Exceptions;
using Mapster;
using Moq;

namespace FriendZonePlus.UnitTests.Services;

public class WallPostServiceTests
{
  #region Setup
  private readonly Mock<IWallPostRepository> _WallPostRepoMock;
  private readonly Mock<IUserRepository> _userRepoMock;
  private readonly WallPostService _sut;


  public WallPostServiceTests()
  {
    // Configure Mapster for tests
    var config = TypeAdapterConfig.GlobalSettings;
    config.Apply(new MappingConfig());

    _WallPostRepoMock = new Mock<IWallPostRepository>();
    _userRepoMock = new Mock<IUserRepository>();
    _sut = new WallPostService(_WallPostRepoMock.Object, _userRepoMock.Object);
  }

  #endregion

  #region CreateAsync

  [Fact]
  public async Task CreateAsync_ShouldReturnWallPostResponseDto_WhenDataIsValid()
  {
    // Arrange
    var currentUserId = 1;
    var dto = new CreateWallPostDto(TargetUserId: 2, Content: "This is a post!");
    var author = new User
    {
      Id = currentUserId,
      Username = "authoruser",
      ProfilePictureUrl = "http://example.com/author.jpg"
    };

    // Simulate that database sets an ID and Author when adding
    _WallPostRepoMock.Setup(x => x.AddAsync(It.IsAny<WallPost>()))
                     .Callback<WallPost>((WallPost p) =>
                     {
                       p.Id = 101;
                       p.AuthorId = currentUserId;
                       p.Author = author;
                       p.CreatedAt = DateTime.UtcNow;
                     })
                     .Returns(Task.CompletedTask);

    // Act
    var result = await _sut.CreateAsync(currentUserId, dto);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(101, result.Id);
    Assert.Equal(dto.Content, result.Content);
    Assert.Equal(currentUserId, result.AuthorId);
    Assert.True(result.CreatedAt > DateTime.MinValue);
    Assert.Equal(author.Username, result.AuthorName);
    Assert.Equal(author.ProfilePictureUrl, result.AuthorProfilePictureUrl);

    _WallPostRepoMock.Verify(x => x.AddAsync(It.Is<WallPost>(wp =>
      wp.AuthorId == currentUserId &&
      wp.TargetUserId == dto.TargetUserId &&
      wp.Content == dto.Content)), Times.Once);
  }

  #endregion

  #region GetFeedAsync

  [Fact]
  public async Task GetFeedAsync_ShouldReturnEmptyList_WhenNoPosts()
  {
    // Arrange
    var currentUserId = 1;
    _WallPostRepoMock.Setup(x => x.GetFeedAsync(currentUserId))
                     .ReturnsAsync(new List<WallPost>());

    // Act
    var result = await _sut.GetFeedAsync(currentUserId);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
    _WallPostRepoMock.Verify(x => x.GetFeedAsync(currentUserId), Times.Once);
  }

  [Fact]
  public async Task GetFeedAsync_ShouldReturnMappedPosts_WhenPostsExist()
  {
    // Arrange
    var currentUserId = 1;
    var now = DateTime.UtcNow;
    var author1 = new User { Id = 2, Username = "user2", ProfilePictureUrl = "http://example.com/user2.jpg" };
    var author2 = new User { Id = 3, Username = "user3", ProfilePictureUrl = "http://example.com/user3.jpg" };

    var posts = new List<WallPost>
    {
        new WallPost
        {
          Id = 1,
          Content = "Post 1",
          AuthorId = 2,
          Author = author1,
          TargetUserId = 1,
          CreatedAt = now.AddMinutes(-5)
        },
        new WallPost
        {
          Id = 2,
          Content = "Post 2",
          AuthorId = 3,
          Author = author2,
          TargetUserId = 1,
          CreatedAt = now
        }
    };

    _WallPostRepoMock.Setup(x => x.GetFeedAsync(currentUserId))
                     .ReturnsAsync(posts);

    // Act
    var result = await _sut.GetFeedAsync(currentUserId);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(2, result.Count);
    Assert.Equal(posts[0].Id, result[0].Id);
    Assert.Equal(posts[0].Content, result[0].Content);
    Assert.Equal(posts[0].AuthorId, result[0].AuthorId);
    Assert.Equal(author1.Username, result[0].AuthorName);
    Assert.Equal(author1.ProfilePictureUrl, result[0].AuthorProfilePictureUrl);
    Assert.Equal(posts[1].Id, result[1].Id);
    Assert.Equal(posts[1].Content, result[1].Content);
    Assert.Equal(posts[1].AuthorId, result[1].AuthorId);
    Assert.Equal(author2.Username, result[1].AuthorName);
    Assert.Equal(author2.ProfilePictureUrl, result[1].AuthorProfilePictureUrl);
    _WallPostRepoMock.Verify(x => x.GetFeedAsync(currentUserId), Times.Once);
  }

  #endregion

  #region GetWallPostsAsync

  [Fact]
  public async Task GetWallPostsAsync_ShouldReturnEmptyList_WhenNoPosts()
  {
    // Arrange
    var userId = 7;
    _WallPostRepoMock.Setup(x => x.GetWallPostsAsync(userId))
                     .ReturnsAsync(new List<WallPost>());

    // Act
    var result = await _sut.GetWallPostsAsync(userId);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
    _WallPostRepoMock.Verify(x => x.GetWallPostsAsync(userId), Times.Once);
  }

  [Fact]
  public async Task GetWallPostsAsync_ShouldReturnMappedPosts_WhenPostsExist()
  {
    // Arrange
    var userId = 7;
    var now = DateTime.UtcNow;
    var author1 = new User { Id = 1, Username = "user1", ProfilePictureUrl = "http://example.com/user1.jpg" };
    var author2 = new User { Id = 2, Username = "user2", ProfilePictureUrl = "http://example.com/user2.jpg" };

    var posts = new List<WallPost>
    {
      new WallPost
      {
        Id = 20,
        Content = "Target post 1",
        AuthorId = 1,
        Author = author1,
        TargetUserId = userId,
        CreatedAt = now.AddMinutes(-3)
      },
      new WallPost
      {
        Id = 21,
        Content = "Target post 2",
        AuthorId = 2,
        Author = author2,
        TargetUserId = userId,
        CreatedAt = now
      }
    };

    _WallPostRepoMock.Setup(x => x.GetWallPostsAsync(userId))
                     .ReturnsAsync(posts);

    // Act
    var result = await _sut.GetWallPostsAsync(userId);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(2, result.Count);
    Assert.Equal(posts[0].Id, result[0].Id);
    Assert.Equal(posts[0].Content, result[0].Content);
    Assert.Equal(posts[0].AuthorId, result[0].AuthorId);
    Assert.Equal(author1.Username, result[0].AuthorName);
    Assert.Equal(author1.ProfilePictureUrl, result[0].AuthorProfilePictureUrl);
    Assert.Equal(posts[1].Id, result[1].Id);
    Assert.Equal(posts[1].Content, result[1].Content);
    Assert.Equal(posts[1].AuthorId, result[1].AuthorId);
    Assert.Equal(author2.Username, result[1].AuthorName);
    Assert.Equal(author2.ProfilePictureUrl, result[1].AuthorProfilePictureUrl);
    _WallPostRepoMock.Verify(x => x.GetWallPostsAsync(userId), Times.Once);
  }

  #endregion

  #region UpdateWallPostAsync

  [Fact]
  public async Task UpdateWallPostAsync_ShouldUpdateWallPost_WhenDataIsValid()
  {
    // Arrange
    var currentUserId = 5;
    var wallPostId = 1;
    var updatedContent = "Updated content";
    var originalCreatedAt = DateTime.UtcNow.AddDays(-1);

    var existingWallPost = new WallPost
    {
      Id = wallPostId,
      AuthorId = currentUserId,
      Content = "Original content",
      TargetUserId = 10,
      CreatedAt = originalCreatedAt
    };

    var dto = new UpdateWallPostDto(updatedContent);

    _WallPostRepoMock.Setup(x => x.GetByIdAsync(wallPostId))
                     .ReturnsAsync(existingWallPost);

    _WallPostRepoMock.Setup(x => x.UpdateAsync(It.IsAny<WallPost>()))
                     .Returns(Task.CompletedTask);

    // Act
    await _sut.UpdateWallPostAsync(currentUserId, wallPostId, dto);

    // Assert
    _WallPostRepoMock.Verify(x => x.GetByIdAsync(wallPostId), Times.Once);
    _WallPostRepoMock.Verify(x => x.UpdateAsync(It.Is<WallPost>(wp =>
      wp.Id == wallPostId &&
      wp.Content == updatedContent &&
      wp.AuthorId == currentUserId)), Times.Once);
  }

  [Fact]
  public async Task UpdateWallPostAsync_ShouldThrowPostNotFoundException_WhenPostDoesNotExist()
  {
    // Arrange
    var currentUserId = 5;
    var wallPostId = 99;
    var dto = new UpdateWallPostDto("Updated content");

    _WallPostRepoMock.Setup(x => x.GetByIdAsync(wallPostId))
                     .ReturnsAsync((WallPost?)null);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<PostNotFoundException>(() =>
      _sut.UpdateWallPostAsync(currentUserId, wallPostId, dto));

    Assert.Equal($"Post with id {wallPostId} was not found.", ex.Message);
    _WallPostRepoMock.Verify(x => x.UpdateAsync(It.IsAny<WallPost>()), Times.Never);
  }

  [Fact]
  public async Task UpdateWallPostAsync_ShouldThrowUnauthorizedPostAccessException_WhenUserIsNotAuthor()
  {
    // Arrange
    var currentUserId = 5;
    var authorId = 10;
    var wallPostId = 1;
    var dto = new UpdateWallPostDto("Updated content");

    var existingWallPost = new WallPost
    {
      Id = wallPostId,
      AuthorId = authorId,
      Content = "Original content",
      TargetUserId = 20,
      CreatedAt = DateTime.UtcNow.AddDays(-1)
    };

    _WallPostRepoMock.Setup(x => x.GetByIdAsync(wallPostId))
                     .ReturnsAsync(existingWallPost);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<UnauthorizedPostAccessException>(() =>
      _sut.UpdateWallPostAsync(currentUserId, wallPostId, dto));

    Assert.Equal("You are not authorized to perform this action on the post.", ex.Message);
    _WallPostRepoMock.Verify(x => x.UpdateAsync(It.IsAny<WallPost>()), Times.Never);
  }

  #endregion

  #region DeleteWallPostAsync

  [Fact]
  public async Task DeleteWallPostAsync_ShouldDeleteWallPost_WhenUserIsAuthor()
  {
    // Arrange
    var currentUserId = 5;
    var wallPostId = 1;

    var existingWallPost = new WallPost
    {
      Id = wallPostId,
      AuthorId = currentUserId,
      Content = "Content to delete",
      TargetUserId = 10,
      CreatedAt = DateTime.UtcNow.AddDays(-1)
    };

    _WallPostRepoMock.Setup(x => x.GetByIdAsync(wallPostId))
                     .ReturnsAsync(existingWallPost);

    _WallPostRepoMock.Setup(x => x.DeleteAsync(It.IsAny<WallPost>()))
                     .Returns(Task.CompletedTask);

    // Act
    await _sut.DeleteWallPostAsync(currentUserId, wallPostId);

    // Assert
    _WallPostRepoMock.Verify(x => x.GetByIdAsync(wallPostId), Times.Once);
    _WallPostRepoMock.Verify(x => x.DeleteAsync(It.Is<WallPost>(wp => wp.Id == wallPostId)), Times.Once);
  }

  [Fact]
  public async Task DeleteWallPostAsync_ShouldDeleteWallPost_WhenUserIsTargetUser()
  {
    // Arrange
    var currentUserId = 10;
    var wallPostId = 1;
    var authorId = 5;

    var existingWallPost = new WallPost
    {
      Id = wallPostId,
      AuthorId = authorId,
      Content = "Content to delete",
      TargetUserId = currentUserId,
      CreatedAt = DateTime.UtcNow.AddDays(-1)
    };

    _WallPostRepoMock.Setup(x => x.GetByIdAsync(wallPostId))
                     .ReturnsAsync(existingWallPost);

    _WallPostRepoMock.Setup(x => x.DeleteAsync(It.IsAny<WallPost>()))
                     .Returns(Task.CompletedTask);

    // Act
    await _sut.DeleteWallPostAsync(currentUserId, wallPostId);

    // Assert
    _WallPostRepoMock.Verify(x => x.GetByIdAsync(wallPostId), Times.Once);
    _WallPostRepoMock.Verify(x => x.DeleteAsync(It.Is<WallPost>(wp => wp.Id == wallPostId)), Times.Once);
  }

  [Fact]
  public async Task DeleteWallPostAsync_ShouldThrowPostNotFoundException_WhenPostDoesNotExist()
  {
    // Arrange
    var currentUserId = 5;
    var wallPostId = 99;

    _WallPostRepoMock.Setup(x => x.GetByIdAsync(wallPostId))
                     .ReturnsAsync((WallPost?)null);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<PostNotFoundException>(() =>
      _sut.DeleteWallPostAsync(currentUserId, wallPostId));

    Assert.Equal($"Post with id {wallPostId} was not found.", ex.Message);
    _WallPostRepoMock.Verify(x => x.DeleteAsync(It.IsAny<WallPost>()), Times.Never);
  }

  [Fact]
  public async Task DeleteWallPostAsync_ShouldThrowUnauthorizedPostAccessException_WhenUserIsNeitherAuthorNorTarget()
  {
    // Arrange
    var currentUserId = 15;
    var authorId = 5;
    var targetUserId = 10;
    var wallPostId = 1;

    var existingWallPost = new WallPost
    {
      Id = wallPostId,
      AuthorId = authorId,
      Content = "Content to delete",
      TargetUserId = targetUserId,
      CreatedAt = DateTime.UtcNow.AddDays(-1)
    };

    _WallPostRepoMock.Setup(x => x.GetByIdAsync(wallPostId))
                     .ReturnsAsync(existingWallPost);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<UnauthorizedPostAccessException>(() =>
      _sut.DeleteWallPostAsync(currentUserId, wallPostId));

    Assert.Equal("You are not authorized to perform this action on the post.", ex.Message);
    _WallPostRepoMock.Verify(x => x.DeleteAsync(It.IsAny<WallPost>()), Times.Never);
  }

  #endregion


}

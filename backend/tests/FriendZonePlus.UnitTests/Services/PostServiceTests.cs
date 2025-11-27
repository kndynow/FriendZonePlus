using System;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;

namespace FriendZonePlus.UnitTests.Services;

public class PostServiceTests
{
  private readonly Mock<IPostRepository> _postRepoMock;
  private readonly Mock<IUserRepository> _userRepoMock;
  private readonly PostService _sut;

  public PostServiceTests()
  {
    _postRepoMock = new Mock<IPostRepository>();
    _userRepoMock = new Mock<IUserRepository>();

    _sut = new PostService(_postRepoMock.Object, _userRepoMock.Object);
  }


  [Theory]
  [InlineData("")]
  [InlineData("     ")]
  public async Task CreatePost_ShouldThrowException_WhenContentIsInvalid(string invalidContent)
  {
    // Arrange
    var dto = new PostDtos.Create(1, 2, invalidContent);
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreatePostAsync(dto));
  }

  [Fact]
  public async Task CreatePost_ShouldThrowException_WhenAuthorDoesNotExist()
  {
    //Arrange
    var dto = new PostDtos.Create(99, 2, "Hej!");

    //Mock so that UserRepo returns null when we query for ID 99
    _userRepoMock.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((User?)null);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreatePostAsync(dto));
    Assert.Equal("Author does not exist", ex.Message);
  }

  [Fact]
  public async Task CreatePost_ShouldReturnPost_WhenDataIsValid()
  {
    //Arrange
    var dto = new PostDtos.Create(1, 2, "Detta är ett bra inlägg!");

    //Setup Users
    _userRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new User());

    //Simulate that database returns an ID
    _postRepoMock.Setup(x => x.AddAsync(It.IsAny<Post>())).ReturnsAsync((Post p) =>
    {
      p.Id = 101;
      return p;
    });

    //Act
    var result = await _sut.CreatePostAsync(dto);

    //Assert
    Assert.Equal(101, result.Id);
    Assert.Equal(dto.Content, result.Content);

    _postRepoMock.Verify(x => x.AddAsync(It.IsAny<Post>()), Times.Once);

  }
}

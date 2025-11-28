using System;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;

namespace FriendZonePlus.UnitTests.Services;

public class WallPostServiceTests
{
  private readonly Mock<IWallPostRepository> _WallPostRepoMock;
  private readonly Mock<IUserRepository> _userRepoMock;
  private readonly WallPostService _sut;

  public WallPostServiceTests()
  {
    _WallPostRepoMock = new Mock<IWallPostRepository>();
    _userRepoMock = new Mock<IUserRepository>();

    _sut = new WallPostService(_WallPostRepoMock.Object, _userRepoMock.Object);
  }


  [Theory]
  [InlineData("")]
  [InlineData("     ")]
  public async Task CreateWallPost_ShouldThrowException_WhenContentIsInvalid(string invalidContent)
  {
    // Arrange
    var dto = new CreateWallPostDto(1, 2, invalidContent);
    // Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateWallPostAsync(dto));
  }

  [Fact]
  public async Task CreateWallPost_ShouldThrowException_WhenAuthorDoesNotExist()
  {
    //Arrange
    var dto = new CreateWallPostDto(99, 2, "Hej!");

    //Mock so that UserRepo returns null when we query for ID 99
    _userRepoMock.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((User?)null);

    // Act & Assert
    var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateWallPostAsync(dto));
    Assert.Equal("Author does not exist", ex.Message);
  }

  [Fact]
  public async Task CreateWallPost_ShouldReturnWallPost_WhenDataIsValid()
  {
    //Arrange
    var dto = new CreateWallPostDto(1, 2, "Detta är ett bra inlägg!");

    //Setup Users
    _userRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new User());

    //Simulate that database returns an ID
    _WallPostRepoMock.Setup(x => x.AddAsync(It.IsAny<WallPost>())).ReturnsAsync((WallPost p) =>
    {
      p.Id = 101;
      return p;
    });

    //Act
    var result = await _sut.CreateWallPostAsync(dto);

    //Assert
    Assert.Equal(101, result.Id);
    Assert.Equal(dto.Content, result.Content);

    _WallPostRepoMock.Verify(x => x.AddAsync(It.IsAny<WallPost>()), Times.Once);

  }
}

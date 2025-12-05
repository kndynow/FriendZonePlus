
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Application.Services.Messages;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;
using Xunit;
namespace FriendZonePlus.UnitTests;


public class MessageServiceTests
{
    private readonly Mock<IMessageRepository> _messageRepoMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly MessageService _messageService;

    public MessageServiceTests()
    {
        _messageRepoMock = new Mock<IMessageRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _messageService = new MessageService(_messageRepoMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldCallRepository_AndReturnMessageResponseDto()
    {
        // Assert
        var senderId = 1;
        var dto = new SendMessageRequestDto(ReceiverId: 2, Content: "Howdy!");

        _messageRepoMock
           .Setup(r => r.AddMessageAsync(It.IsAny<Message>()))
           .ReturnsAsync((Message m) =>
           {
             m.Id = 1; 
             return m;
           });

        _userRepositoryMock
           .Setup(r => r.ExistsByIdAsync(dto.ReceiverId))
           .ReturnsAsync(true);

        _userRepositoryMock
         .Setup(r => r.ExistsByIdAsync(senderId))
        .ReturnsAsync(true);

        // Act
        var expected = await _messageService.SendMessageAsync(senderId, dto);

        // Assert
        Assert.NotNull(expected);
        Assert.Equal(1, expected.Id);
        Assert.Equal(senderId, expected.SenderId);
        Assert.Equal(dto.ReceiverId, expected.ReceiverId);
        Assert.Equal(dto.Content, expected.Content);

        _messageRepoMock.Verify(r => r.AddMessageAsync(It.IsAny<Message>()), Times.Once);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldThrowError_WhenReceiverIdDoesntExist()
    {
        // Assert
        var senderId = 1;
        var dto = new SendMessageRequestDto(ReceiverId: 2, Content: "Howdy!");

        _userRepositoryMock
        .Setup(r => r.ExistsByIdAsync(senderId))
        .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(r => r.ExistsByIdAsync(dto.ReceiverId))
            .ReturnsAsync(false);

        // Act & Assert
        var expected = await Assert.ThrowsAsync<ArgumentException>(() =>
            _messageService.SendMessageAsync(senderId, dto)
        );

        Assert.Equal("Receiver does not exist", expected.Message);

        _messageRepoMock.Verify(
            r => r.AddMessageAsync(It.IsAny<Message>()),
            Times.Never
        );
    }

    [Fact]
    public async Task SendMessageAsync_ShouldThrowError_WhenSenderIdDoesntExist()
    {
        // Assert
        var senderId = 1;
        var dto = new SendMessageRequestDto(ReceiverId: 2, Content: "Howdy!");

        _userRepositoryMock
            .Setup(r => r.ExistsByIdAsync(senderId))
            .ReturnsAsync(false);

        _userRepositoryMock
         .Setup(r => r.ExistsByIdAsync(dto.ReceiverId))
         .ReturnsAsync(true);

        // Act & Assert
        var expected = await Assert.ThrowsAsync<ArgumentException>(() =>
            _messageService.SendMessageAsync(senderId, dto)
        );

        Assert.Equal("Sender does not exist", expected.Message);

        _messageRepoMock.Verify(
            r => r.AddMessageAsync(It.IsAny<Message>()),
            Times.Never
        );
    }
}

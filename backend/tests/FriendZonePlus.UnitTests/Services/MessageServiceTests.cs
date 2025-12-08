
using FluentValidation;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Interfaces;
using FriendZonePlus.Application.Services;
using FriendZonePlus.Application.Services.Messages;
using FriendZonePlus.Application.Validators;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using Moq;
using Xunit;
namespace FriendZonePlus.UnitTests;


public class MessageServiceTests
{
    private readonly Mock<IMessageRepository> _messageRepoMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMessageNotifier> _notifierMock;
    private readonly IValidator<SendMessageRequestDto> _validator;
    private readonly MessageService _messageService;

    public MessageServiceTests()
    {
        _messageRepoMock = new Mock<IMessageRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _notifierMock = new Mock<IMessageNotifier>();
        _validator = new SendMessageRequestDtoValidator();
        _messageService = new MessageService(_messageRepoMock.Object, _userRepositoryMock.Object, _validator, _notifierMock.Object);
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
    public async Task SendMessageAsync_ShouldThrowError_WhenUserSendsToItself()
    {
        // Assert
        var senderId = 1;
        var dto = new SendMessageRequestDto(ReceiverId: 1, Content: "Howdy!");

        _userRepositoryMock
        .Setup(r => r.ExistsByIdAsync(senderId))
        .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(r => r.ExistsByIdAsync(dto.ReceiverId))
            .ReturnsAsync(true);

        // Act & Assert
        var expected = await Assert.ThrowsAsync<ArgumentException>(() =>
            _messageService.SendMessageAsync(senderId, dto)
        );

        Assert.Equal("User cannot send message to itself", expected.Message);

        _messageRepoMock.Verify(
            r => r.AddMessageAsync(It.IsAny<Message>()),
            Times.Never
        );
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

    [Fact]
    public async Task GetMessagesBetweenUsersAsync_ShouldReturnOrderedMessages()
    {
        // Arrange
        int senderId = 1;
        int receiverId = 2;

        _userRepositoryMock.Setup(r => r.ExistsByIdAsync(senderId)).ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.ExistsByIdAsync(receiverId)).ReturnsAsync(true);

        var messagesFromRepo = new List<Message>
        {
            new Message { Id = 1, SenderId = 1, ReceiverId = 2, Content = "Hi", SentAt = DateTime.UtcNow.AddMinutes(-2) },
            new Message { Id = 2, SenderId = 2, ReceiverId = 1, Content = "Hello", SentAt = DateTime.UtcNow.AddMinutes(-1) }
        };

        _messageRepoMock
            .Setup(r => r.GetMessagesBetweenUsersAsync(senderId, receiverId))
            .ReturnsAsync(messagesFromRepo);

        // Act
        var result = await _messageService.GetMessagesBetweenUsersAsync(senderId, receiverId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Hi", result.ElementAt(0).Content);
        Assert.Equal("Hello", result.ElementAt(1).Content);

        _messageRepoMock.Verify(r => r.GetMessagesBetweenUsersAsync(senderId, receiverId), Times.Once);
    }

    [Fact]
    public async Task GetMessagesBetweenUsersAsync_ShouldThrowError_WhenSenderDoesNotExist()
    {
        // Arrange
        var senderId = 1;
        var receiverId = 2;

        _userRepositoryMock.Setup(r => r.ExistsByIdAsync(senderId)).ReturnsAsync(false);
        _userRepositoryMock.Setup(r => r.ExistsByIdAsync(receiverId)).ReturnsAsync(true);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            _messageService.GetMessagesBetweenUsersAsync(senderId, receiverId));

        Assert.Equal("Cannot retrieve messages for the same user", ex.Message);
        _messageRepoMock.Verify(r => r.GetMessagesBetweenUsersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetMessagesBetweenUsersAsync_ShouldThrowError_WhenReceiverDoesNotExist()
    {
        // Arrange
        var senderId = 1;
        var receiverId = 2;

        _userRepositoryMock.Setup(r => r.ExistsByIdAsync(senderId)).ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.ExistsByIdAsync(receiverId)).ReturnsAsync(false);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            _messageService.GetMessagesBetweenUsersAsync(senderId, receiverId));

        Assert.Equal("Receiver does not exist", ex.Message);
        _messageRepoMock.Verify(r => r.GetMessagesBetweenUsersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task SendMessageAsync_ShouldThrowValidationException_WhenContentIsInvalid(string content)
    {
        // Arrange
        var senderId = 1;
        var dto = new SendMessageRequestDto(ReceiverId: 2, Content: content);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _messageService.SendMessageAsync(senderId, dto)
        );

        // Checks  the exception contains the correct error message
        Assert.Contains(ex.Errors, e => e.PropertyName == nameof(dto.Content));
    }

    [Fact]
    public async Task SendMessageAsync_ShouldThrowValidationException_WhenContentTooLong()
    {
        // Arrange
        var senderId = 1;
        var longContent = new string('a', 301); // 301 characters
        var dto = new SendMessageRequestDto(ReceiverId: 2, Content: longContent);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _messageService.SendMessageAsync(senderId, dto)
        );

        // Checks that the exception contains the correct error message
        Assert.Contains(ex.Errors, e => e.PropertyName == nameof(dto.Content));
    }

    [Fact]
    public async Task GetLatestChatsAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var userId = 1;
        var messages = new List<Message>
    {
        new Message { Id = 1, SenderId = userId, ReceiverId = 2, Content = "Hi", SentAt = DateTime.UtcNow, IsRead = false },
        new Message { Id = 2, SenderId = 3, ReceiverId = userId, Content = "Hello", SentAt = DateTime.UtcNow, IsRead = false }
    };

        _messageRepoMock.Setup(r => r.GetLatestMessagesForUserAsync(userId))
                        .ReturnsAsync(messages);

        // Act
        var result = await _messageService.GetLatestChatsAsync(userId);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, m => m.Id == 1 && m.Content == "Hi");
        Assert.Contains(result, m => m.Id == 2 && m.Content == "Hello");
    }

    [Fact]
    public async Task SendMessageAsync_ShouldNotifyUsers_WhenMessageIsSaved()
    {
        // Arrange
        var senderId = 1;
        var dto = new SendMessageRequestDto(ReceiverId: 2, Content: "Hello!");

        _userRepositoryMock.Setup(r => r.ExistsByIdAsync(senderId)).ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.ExistsByIdAsync(dto.ReceiverId)).ReturnsAsync(true);

        _messageRepoMock.Setup(r => r.AddMessageAsync(It.IsAny<Message>()))
            .ReturnsAsync((Message m) => { m.Id = 1; return m; });

        // Act
        var result = await _messageService.SendMessageAsync(senderId, dto);

        // Assert
        _notifierMock.Verify(n => n.NotifyMessageSentAsync(
            senderId,
            dto.ReceiverId,
            It.Is<MessageResponseDto>(msg => msg.Content == dto.Content)
        ), Times.Once);
    }
}

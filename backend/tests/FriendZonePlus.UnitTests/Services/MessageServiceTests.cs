
using FriendZonePlus.Application.Services.Messages;
using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Core.Entities;
using Moq;
using Xunit;
namespace FriendZonePlus.UnitTests;


public class MessageServiceTests
{
    private readonly Mock<IMessageRepository> _messageRepoMock;
    private readonly MessageService _messageService;

    public MessageServiceTests()
    {
        _messageRepoMock = new Mock<IMessageRepository>();
        _messageService = new MessageService(_messageRepoMock.Object);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldCallRepository_AndReturnMessageWithId()
    {
        // Assert
        var senderId = 1;
        var receiverId = 2;
        var content = "Howdy!";

        var expectedMessage = new Message
        {
            Id = 1,
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            SentAt = DateTime.UtcNow
        };

        _messageRepoMock
         .Setup(r => r.AddMessageAsync(It.IsAny<Message>()))
         .ReturnsAsync((Message m) =>
         {
             m.Id = 1; 
             return m;
         });

        // Act
        var result = await _messageService.SendMessageAsync(senderId, receiverId, content);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(senderId, result.SenderId);
        Assert.Equal(receiverId, result.ReceiverId);
        Assert.Equal(content, result.Content);

        _messageRepoMock.Verify(r => r.AddMessageAsync(It.IsAny<Message>()), Times.Once);
    }
}

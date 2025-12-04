using FriendZonePlus.Core.Entities;

namespace FriendZonePlus.Application.Services.Messages
{
    public interface IMessageService
    {
        Task<Message> SendMessageAsync(int senderId, int recieverId, string content);
    }
}
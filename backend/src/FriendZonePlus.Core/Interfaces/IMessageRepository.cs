using FriendZonePlus.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendZonePlus.Core.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message> AddMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(int senderId, int receiverId);
        Task<IEnumerable<Message>> GetLatestMessagesForUserAsync(int userId);

    }
}

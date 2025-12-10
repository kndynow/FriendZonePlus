using FriendZonePlus.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendZonePlus.Application.Interfaces
{
    // For signalR notification
    public interface IMessageNotifier
    {
        Task NotifyMessageSentAsync(int senderId, int receiverId, MessageResponseDto message);
    }
}

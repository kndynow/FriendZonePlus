using FriendZonePlus.API.Hubs;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

public class SignalRMessageNotifier : IMessageNotifier
{
    private readonly IHubContext<MessageHub> _hubContext;
    public SignalRMessageNotifier(IHubContext<MessageHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyMessageSentAsync(int senderId, int receiverId, MessageResponseDto message)
    {
        await _hubContext.Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", message);
        await _hubContext.Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", message);
    }
}

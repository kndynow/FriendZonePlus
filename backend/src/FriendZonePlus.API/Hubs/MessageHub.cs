using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FriendZonePlus.API.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {

    }
}
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class SessionUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        var userId = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
    {
            return null;
    }
        return userId; 
    }
}
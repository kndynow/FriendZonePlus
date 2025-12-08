using FriendZonePlus.Application.Services.Messages;
using FriendZonePlus.Application.DTOs;

using System.Security.Claims;

namespace FriendZonePlus.API.Endpoints
{
    public static class MessageEndpoints
    {
        public static void MapMessageEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Message")
                .WithTags("Message")
                .RequireAuthorization();

            group.MapPost("send", SendMessage)
                .WithDescription("Sends a message to another user")
                .WithSummary("Send message");

            group.MapGet("conversation/{receiverId:int}", GetMessagesBetweenUsers)
                .WithDescription("Gets all messages in a conversation between the authenticated user and a specific receiver")
                .WithSummary("Get conversation");

            group.MapGet("latest", GetLatestChats)
                .WithDescription("Gets the latest chat conversations for the authenticated user, showing the most recent message from each conversation")
                .WithSummary("Get latest chats");
        }

        private static async Task<IResult> SendMessage(
            IMessageService messageService,
            SendMessageRequestDto requestDto,
            ClaimsPrincipal user)
        {
            try
            {
                // Checks if the user is authenticated and retrieves senderId from claims
                var senderIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(senderIdClaim) || !int.TryParse(senderIdClaim, out int senderId))
                {
                    return Results.Unauthorized();
                }

                var response = await messageService.SendMessageAsync(senderId, requestDto);

                return Results.Created($"/api/Message/{response.Id}", response);
            }

            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return Results.BadRequest(new { message = "Unable to send message." });
            }
        }

        private static async Task<IResult> GetMessagesBetweenUsers(IMessageService messageService, ClaimsPrincipal user, int receiverId)
        {
            try
            {
                // Checks if the user is authenticated and retrieves senderId from claims
                var senderIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(senderIdClaim) || !int.TryParse(senderIdClaim, out int senderId))
                {
                    return Results.Unauthorized();
                }

                var response = await messageService.GetMessagesBetweenUsersAsync(senderId, receiverId);

                return Results.Ok(response);
            }

            catch (UnauthorizedAccessException)
            {
                return Results.Forbid();
            }

            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return Results.BadRequest(new { message = "Unable to send message." });
            }
        }
        private static async Task<IResult> GetLatestChats(
         IMessageService messageService,
         ClaimsPrincipal user)
        {
            try
            {
                // Checks if the user is authenticated and retrieves senderId from claims
                var senderIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(senderIdClaim) || !int.TryParse(senderIdClaim, out int senderId))
                {
                    return Results.Unauthorized();
                }

                var response = await messageService.GetLatestChatsAsync(senderId);

                return Results.Ok(response);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return Results.BadRequest(new { message = "Unable to retrieve latest chats." });
            }
        }
    }
}

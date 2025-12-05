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
                .WithTags("Message");

            group.MapPost("send", SendMessage);
            //group.MapGet("get", GetMessagesBetweenUsers);
        }

        private static async Task<IResult> SendMessage(
            IMessageService messageService,
            SendMessageRequestDto requestDto,
            ClaimsPrincipal user)
        {
            try
            {
                //var senderIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if (string.IsNullOrEmpty(senderIdClaim) || !int.TryParse(senderIdClaim, out int senderId))
                //{
                //    return Results.Unauthorized();
                //}

                // Temporary until JWT is in place
                var senderId = 1;

                var response = await messageService.SendMessageAsync(senderId, requestDto);

                return Results.Created($"/api/Message/{response.Id}", response);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return Results.BadRequest( new { message = "Unable to send message." });
            }
        }

        //private static Task<IEnumerable<MessageResponseDto>> GetMessagesBetweenUsers(IMessageService messageService, ClaimsPrincipal user)
        //{

        //}
    }
}

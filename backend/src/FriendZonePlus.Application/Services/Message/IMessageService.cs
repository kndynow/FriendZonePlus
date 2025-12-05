using FriendZonePlus.Core.Entities;
using FriendZonePlus.Application.DTOs;

namespace FriendZonePlus.Application.Services.Messages
{
    public interface IMessageService
    {
        Task<MessageResponseDto> SendMessageAsync(int senderId, SendMessageRequestDto Dto);
        Task<IEnumerable<MessageResponseDto>> GetMessagesBetweenUsersAsync(int senderUserId, int retreivingUserId);
    }
}
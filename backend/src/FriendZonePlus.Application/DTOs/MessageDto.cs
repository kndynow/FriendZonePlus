using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FriendZonePlus.Application.DTOs
{
    public record SendMessageRequestDto(
        [Required, Range (1, int.MaxValue, ErrorMessage = "Receiver Id must be greater than 0")]
        int ReceiverId,

        [Required, MinLength(1, ErrorMessage = "Message cannot be empty"),
         MaxLength(300, ErrorMessage = "Message cannot exceed 300 characters")]
        string Content
    );

    public record MessageResponseDto(
        int Id,
        int SenderId,
        int ReceiverId,
        string Content,
        DateTime SentAt,
        bool IsRead
     );
}

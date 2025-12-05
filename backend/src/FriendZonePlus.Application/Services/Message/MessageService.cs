using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace FriendZonePlus.Application.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        public MessageService(IMessageRepository messageRepository, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        public async Task<MessageResponseDto> SendMessageAsync(int senderId, SendMessageRequestDto dto)
        {

            if (!await _userRepository.ExistsByIdAsync(dto.ReceiverId))
                throw new ArgumentException("Receiver does not exist");

            if (!await _userRepository.ExistsByIdAsync(senderId))
                throw new ArgumentException("Sender does not exist");

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
            };

            var response = await _messageRepository.AddMessageAsync(message);

            return new MessageResponseDto(
                response.Id,
                response.SenderId,
                response.ReceiverId,
                response.Content,
                response.SentAt,
                response.IsRead
            );
        }
    }
}

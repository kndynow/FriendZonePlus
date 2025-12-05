using FluentValidation;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Core.Entities;
using FriendZonePlus.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace FriendZonePlus.Application.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<SendMessageRequestDto> _validator;

        public MessageService(IMessageRepository messageRepository, IUserRepository userRepository, IValidator<SendMessageRequestDto> validator)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<MessageResponseDto> SendMessageAsync(int senderId, SendMessageRequestDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            if (senderId == dto.ReceiverId)
                throw new ArgumentException("User cannot send message to itself");

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
        public async Task<IEnumerable<MessageResponseDto>> GetMessagesBetweenUsersAsync(int senderId, int receiverId)
        {
            if (!await _userRepository.ExistsByIdAsync(receiverId))
                throw new ArgumentException("Receiver does not exist");

            if (!await _userRepository.ExistsByIdAsync(senderId))
                throw new ArgumentException("Cannot retrieve messages for the same user");

            var messages = await _messageRepository.GetMessagesBetweenUsersAsync(senderId, receiverId);

            var responseDtos = messages
              .OrderBy(m => m.SentAt) 
              .Select(m => new MessageResponseDto(
                  Id: m.Id,
                  SenderId: m.SenderId,
                  ReceiverId: m.ReceiverId,
                  Content: m.Content,
                  SentAt: m.SentAt,
                  IsRead: m.IsRead
               ));

            return responseDtos;
        }
    }
}

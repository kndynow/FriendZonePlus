using FluentValidation;
using FriendZonePlus.Application.DTOs;
using FriendZonePlus.Application.Interfaces;
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
        private readonly IMessageNotifier _notifier;
        private readonly IValidator<SendMessageRequestDto> _validator;

        public MessageService(IMessageRepository messageRepository, IUserRepository userRepository, IValidator<SendMessageRequestDto> validator, IMessageNotifier notifier)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _validator = validator;
            _notifier = notifier;
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

            if (await _userRepository.GetByIdAsync(dto.ReceiverId) is null)
                throw new ArgumentException("Receiver does not exist");

            if (await _userRepository.GetByIdAsync(senderId) is null)
                throw new ArgumentException("Sender does not exist");

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
            };

            var savedMessage = await _messageRepository.AddMessageAsync(message);

            var responseDto = new MessageResponseDto(
                savedMessage.Id,
                savedMessage.SenderId,
                savedMessage.ReceiverId,
                savedMessage.Content,
                savedMessage.SentAt,
                savedMessage.IsRead
            );

            // Notifies users via signalR
            await _notifier.NotifyMessageSentAsync(senderId, dto.ReceiverId, responseDto);

            return responseDto;
        }

        public async Task<IEnumerable<MessageResponseDto>> GetMessagesBetweenUsersAsync(int senderId, int receiverId)
        {
            if (await _userRepository.GetByIdAsync(receiverId) is null)
                throw new ArgumentException("Receiver does not exist");

            if (await _userRepository.GetByIdAsync(senderId) is null)
                throw new ArgumentException("Sender does not exist");

            var messages = await _messageRepository.GetMessagesBetweenUsersAsync(senderId, receiverId);

            var responseDtos = messages
              .OrderBy(m => m.SentAt)
              .Select(m => new MessageResponseDto(
                  m.Id,
                  m.SenderId,
                  m.ReceiverId,
                  m.Content,
                  m.SentAt,
                  m.IsRead
               ));

            return responseDtos;
        }

        public async Task<IEnumerable<MessageResponseDto>> GetLatestChatsAsync(int userId)
        {
            var messages = await _messageRepository.GetLatestMessagesForUserAsync(userId);

            return messages.Select(m => new MessageResponseDto(
                m.Id,
                m.SenderId,
                m.ReceiverId,
                m.Content,
                m.SentAt,
                m.IsRead
            ));
        }
    }
}

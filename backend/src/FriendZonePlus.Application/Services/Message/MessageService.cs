using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace FriendZonePlus.Application.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Message> SendMessageAsync(int senderId, int receiverId, string content)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            return await _messageRepository.AddMessageAsync(message);
        }
    }
}

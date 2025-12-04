using FriendZonePlus.Core.Interfaces;
using FriendZonePlus.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace FriendZonePlus.Application.Services.Messages
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService (IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Message> SendMessageAsync(int senderId, int recieverId, string content)
        {
            throw new NotImplementedException();
        }
    }
}

using FriendZonePlus.Infrastructure.Data;
using FriendZonePlus.Core.Interfaces;
using System;
using FriendZonePlus.Core.Entities;
using System.Collections.Generic;
using System.Text;

namespace FriendZonePlus.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly FriendZonePlusContext _context;

        public MessageRepository(FriendZonePlusContext context)
        {
            _context = context;
        }

        public async Task<Message> AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);

            await _context.SaveChangesAsync();

            return message;
        }
    }
}

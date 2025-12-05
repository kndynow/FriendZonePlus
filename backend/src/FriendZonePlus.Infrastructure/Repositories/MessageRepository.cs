using FriendZonePlus.Infrastructure.Data;
using FriendZonePlus.Core.Interfaces;
using System;
using FriendZonePlus.Core.Entities;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(int senderUserId, int receivingUserId)
        {
            return await _context.Messages
                .Where(m =>
                    (m.SenderId == senderUserId && m.ReceiverId == receivingUserId) ||
                    (m.SenderId == receivingUserId && m.ReceiverId == senderUserId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();                
        }
     }
}

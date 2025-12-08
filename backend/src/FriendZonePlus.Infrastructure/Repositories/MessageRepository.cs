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

        public async Task<IEnumerable<Message>> GetMessagesBetweenUsersAsync(int senderId, int receiverId)
        {
            return await _context.Messages
                .Where(m =>
                    (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                    (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetLatestMessagesForUserAsync(int userId)
        {
            var messages = await _context.Messages
              .Where(m => m.SenderId == userId || m.ReceiverId == userId)
              .OrderByDescending(m => m.SentAt)
              .ToListAsync();

            var latestChats = messages
              .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
              .Select(g => g.Take(1).First())
              .OrderByDescending(m => m.SentAt);

            return latestChats;
        }
    }
}

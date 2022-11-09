using ChatApp_SignalR.Data;
using ChatApp_SignalR.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp_SignalR.Infrastructure.Repository
{
    public class ChatRepository : IChatRepository
    {
        private ApplicationDbContext _ctx;

        public ChatRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<Message> CreateMessage(int chatId, string message, string userId)
        {
            var Message = new Message
            {
                ChatId = chatId,
                Text = message,
                Name = userId,
                Timestamp = DateTime.Now
            };
            _ctx.Messages.Add(Message);
            await _ctx.SaveChangesAsync();

            return Message;
        }

        public async Task<int> CreatePrivateRoom(string userName, string rootId, string targetId)
        {
            var chat = new Chat
            {
                Name = userName,
                Type = ChatType.Private
            };

            chat.ChatUser.Add(new ChatUser
            {
                UserId = rootId
            });
            chat.ChatUser.Add(new ChatUser
            {
                UserId = targetId
            });

            _ctx.Chats.Add(chat);

            await _ctx.SaveChangesAsync();

            return chat.Id;
        }

        public async Task CreateRoom(string name, string userId)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room
            };

            chat.ChatUser.Add(new ChatUser
            {
                UserId = userId,
                Role = UserRole.Admin
            });

            _ctx.Chats.Add(chat);
            await _ctx.SaveChangesAsync();
        }

        public Chat GetChat(int id)
        {
            return _ctx.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Chat> GetChats(string userId)
        {
            return _ctx.Chats
                .Include(x => x.ChatUser)
                .Where(x => !x.ChatUser
                .Any(y => y.UserId == userId))
                .ToList();
        }

        public IEnumerable<Chat> GetPrivateChats(string userId)
        {
            return _ctx.Chats
                .Include(x => x.ChatUser)
                .ThenInclude(x => x.User)
                .Where(x => x.Type == ChatType.Private && x.ChatUser
                .Any(y => y.UserId == userId))
                .ToList();
        }

        public async Task JoinRoom(int chatId, string userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                Role = UserRole.Member
            };
            _ctx.ChatUsers.Add(chatUser);

            await _ctx.SaveChangesAsync();
        }

        public async Task AddUserToGroup(int chatId, string userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                Role = UserRole.Member
            };
            _ctx.ChatUsers.Add(chatUser);

            await _ctx.SaveChangesAsync();
        }
    }
}

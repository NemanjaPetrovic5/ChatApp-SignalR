using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp_SignalR.Models
{
    public class Chat
    {
        public Chat()
        {
            Messages = new List<Message>();
            ChatUser = new List<ChatUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ChatType Type { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatUser> ChatUser { get; set; }
    }
}

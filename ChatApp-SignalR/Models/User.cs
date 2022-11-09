﻿ using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp_SignalR.Models
{
    public class User : IdentityUser
    {
        public User() : base()
        {
            ChatUser = new List<ChatUser>();
        }
        public ICollection<ChatUser> ChatUser { get; set; }
    }
}

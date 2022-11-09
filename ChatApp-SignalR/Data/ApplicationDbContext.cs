using ChatApp_SignalR.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp_SignalR.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<ChatUser>()
            //    .HasKey(x => new { x.ChatId, x.UserId });

            builder.Entity<ChatUser>()
       .HasKey(ub => new { ub.ChatId, ub.UserId });

            builder.Entity<ChatUser>()
                .HasOne(ub => ub.User)
                .WithMany(au => au.ChatUser)
                .HasForeignKey(ub => ub.UserId);

            builder.Entity<ChatUser>()
                .HasOne(ub => ub.Chatss)
                .WithMany(au => au.ChatUser) 
                .HasForeignKey(ub => ub.ChatId);
        }
    }
}

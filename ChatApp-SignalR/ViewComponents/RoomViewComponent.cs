using ChatApp_SignalR.Data;
using ChatApp_SignalR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApp_SignalR.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private ApplicationDbContext _db;
        public RoomViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var chats = _db.ChatUsers
                .Include(x => x.Chatss)
                .Where(x => x.UserId == userId 
                    && x.Chatss.Type == ChatType.Room)
                .Select(x => x.Chatss)
                .ToList();
            return View(chats);
        }
    }
}

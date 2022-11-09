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

    public class PrivateViewComponent : ViewComponent
    {
        private ApplicationDbContext _db;
        public PrivateViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }
        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var chats =  _db.Chats
                .Include(x => x.ChatUser)
                .ThenInclude(x => x.User)
                .Where(x => x.Type == ChatType.Private && x.ChatUser
                .Any(y => y.UserId == userId))
                .ToList();
            return View(chats);

        }
    }
}

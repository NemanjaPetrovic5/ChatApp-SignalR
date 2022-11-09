using ChatApp_SignalR.Data;
using ChatApp_SignalR.Hubs;
using ChatApp_SignalR.Infrastructure;
using ChatApp_SignalR.Infrastructure.Repository;
using ChatApp_SignalR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApp_SignalR.Controllers
{
    [Authorize]

    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private IChatRepository _repo;


        public HomeController(ILogger<HomeController> logger, IChatRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult Index([FromServices] ApplicationDbContext ctx)
        {
            var chat =  ctx.Chats

                .ToList();
            return View();
        }

        public IActionResult Find([FromServices] ApplicationDbContext ctx)
        {
            var users = ctx.Users
                .Where(x => x.Id != User.GetUserId())
                .ToList();

            return View(users);
        }
       
        //public IActionResult Private([FromServices] ApplicationDbContext ctx, string searchString)
        //{
        //    ViewData["CurrentFilter"] = searchString;
        //    var students = from s in ctx.Chats
        //                   select s;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        students = students.Where(s => s.Name.Contains(searchString));
        //    }
        //    var chats = _repo.GetPrivateChats(User.GetUserId());
        //    return View(chats);
        //}
        public async Task<IActionResult> Private([FromServices] ApplicationDbContext ctx, string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var students = from s in ctx.Chats
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name.Contains(searchString));
            }
         
            return View(await students.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> CreatePrivateRoom(string userId, [FromServices] ApplicationDbContext ctx)
        {
            var userName = ctx.Users
                .Where(x => x.Id == userId)
                .FirstOrDefault();

            var id = await _repo.CreatePrivateRoom(userName.ToString(),GetUserId(), userId);

            return RedirectToAction("Chat", new { id });
        }
        [HttpGet("{id}")]
        public IActionResult Chat(int id, [FromServices] ApplicationDbContext ctx)
        {
            var chat = _repo.GetChat(id);

            var downtimeJoin = from d in ctx.Users
                               join dr in ctx.ChatUsers on d.Id equals dr.UserId
                               join c in ctx.Chats on dr.ChatId equals c.Id
                               //where c.Type == ChatType.Private
                               where c.Id == id
                               where d.Id != GetUserId()
                               select new User
                               {
                                   Id = d.Id,
                                   UserName = d.UserName,
                               };
       
            ViewData["UserGroups1"] = downtimeJoin;

            var downtimeJoin1 = from d in ctx.Users
                                    join dr in ctx.ChatUsers on d.Id equals dr.UserId
                                    join c in ctx.Chats on dr.ChatId equals c.Id
                                    where d.Id != GetUserId()
                                    select new User
                                    {
                                        Id = d.Id,
                                        UserName = d.UserName,
                                    };

                ViewData["UserGroups2"] = downtimeJoin1.Distinct();
            return View(chat);
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateMessage(int chatId, string message)
        //{
        //    var Message = new Message
        //    {
        //        ChatId = chatId,
        //        Text = message,
        //        Name = User.Identity.Name,
        //        Timestamp = DateTime.Now
        //    };
        //    _ctx.Messages.Add(Message);
        //    await _ctx.SaveChangesAsync();
        //    return RedirectToAction("Chat",new { id = chatId});
        //}
        public async Task<IActionResult> CreateRoom(string name)
        {
            await _repo.CreateRoom(name, GetUserId());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinChat(int id)
        {

            await _repo.JoinRoom(id, GetUserId());
            return RedirectToAction("Chat", "Home", new { id = id });

        }

        public async Task<IActionResult> AddUserToGroup(int id, string UserId)
        {

            await _repo.AddUserToGroup(id, UserId);
            return RedirectToAction("Chat", "Home", new { id = id });

        }
        public async Task<IActionResult> SendMessage(
            int roomId,
            string message,
            [FromServices] IHubContext<ChatHub> chat)
        {
            var Message = await _repo.CreateMessage(roomId, message, User.Identity.Name);

            await chat.Clients.Group(roomId.ToString())
                .SendAsync("RecieveMessage", new
                {
                    Text = Message.Text,
                    Name = Message.Name,
                    Timestamp = Message.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                });

            return Ok();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

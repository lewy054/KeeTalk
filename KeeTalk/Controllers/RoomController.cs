using KeeTalk.Data;
using KeeTalk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeeTalk.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        [BindProperty]
        public IEnumerable<Message> Messages { get; set; }
        [BindProperty]
        public Message Message { get; set; }


        public RoomController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: ChatController
        public ActionResult Index(int id)
        {
            Messages = _context.Messages.Where(u => u.RoomId == id);
            ChatRoom chatRoom = new ChatRoom();
            chatRoom = _context.ChatRoom.FirstOrDefault(u => u.Id == id);
            foreach (var message in Messages)
            {
                message.AuthorProfileImage = _context.Users.FirstOrDefault(u => u.Id == message.AuthorId).ImageName;
                message.AuthorName = _userManager.Users.Where(u => u.Id == message.AuthorId).FirstOrDefault().UserName;
            }
            ChatRoomMultipleModel ChatRoomMultipleModelList = new ChatRoomMultipleModel
            {
                Message = new Message { RoomId = id },
                Messages = Messages,
                ChatRoom = chatRoom
            };
            return View(ChatRoomMultipleModelList);
        }

        // POST: ChatController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            if (ModelState.IsValid)
            {
                Message.Date = DateTime.Now;
                Message.AuthorId = _userManager.GetUserId(User);
                byte[] bytes = Encoding.Default.GetBytes(Message.Content);
                Message.Content = Encoding.UTF8.GetString(bytes);
                _context.Messages.Add(Message);
                _context.SaveChanges();
                return RedirectToAction("Index", new { id = Message.RoomId });
            }
            return View(Message);
        }
    }
}

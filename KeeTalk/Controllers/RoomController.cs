using KeeTalk.Data;
using KeeTalk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public IEnumerable<Message> Messages { get; set; }
        [BindProperty]
        public Message Message { get; set; }


        public RoomController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: ChatController
        public ActionResult Index(int id)
        {
            Messages = _db.Messages.Where(u => u.RoomId == id);
            ChatRoom chatRoom = new ChatRoom();
            chatRoom = _db.ChatRoom.FirstOrDefault(u => u.Id == id);
            foreach (var item in Messages)
            {
                item.AuthorProfileImage = _db.Users.FirstOrDefault(u => u.UserName == item.Author).ImageName;
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
            if (!String.IsNullOrEmpty(User.Identity.Name))
            {
                Message.Date = DateTime.Now;
                Message.Author = User.Identity.Name;
                byte[] bytes = Encoding.Default.GetBytes(Message.Content);
                Message.Content = Encoding.UTF8.GetString(bytes);
                _db.Messages.Add(Message);
                _db.SaveChanges();
                return RedirectToAction("Index", new { id = Message.RoomId });

            }
            return View("NotAuthorized");


        }
    }
}

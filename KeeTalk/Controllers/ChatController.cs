using KeeTalk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeeTalk.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _db;
        private static IEnumerable<Message> messages;

        public ChatController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: ChatController
        public ActionResult Index()
        {
            messages = _db.Messages.ToList();
            return View(messages);
        }

        // GET: ChatController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChatController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Message message)
        {
            message.Date = DateTime.Now;
            message.Author = "dsa";
            _db.Messages.Add(message);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

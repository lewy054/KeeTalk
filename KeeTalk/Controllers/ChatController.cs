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
        [BindProperty]
        public IEnumerable<Message> Messages { get; set; }
        [BindProperty]
        public Message Message { get; set; }
        

        public ChatController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: ChatController
        public ActionResult Index()
        {
            Messages = _db.Messages.ToList();
            return View(Messages);
        }

        // POST: ChatController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            Message.Date = DateTime.Now;
            Message.Author = "dsa";
            _db.Messages.Add(Message);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

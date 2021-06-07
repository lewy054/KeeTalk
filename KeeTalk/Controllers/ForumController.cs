using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KeeTalk.Data;
using KeeTalk.Models;

namespace KeeTalk.Controllers
{
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext _context;
        public Thread OneThread { get; set; }
        public IEnumerable<Thread> Threads { get; set; }
        public ForumController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Forum
        public IActionResult Index()
        {
            return View();
        }

        // GET: Threads/Create
        public IActionResult Create()
        {
            string referer = Request.Headers["Referer"].ToString();
            Uri baseUri = new Uri(referer);
            string section = baseUri.Segments[^1];
            Thread Thread = new Thread { Section = section };
            return View(Thread);
        }

        // POST: Threads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Creator,Section")] Thread thread)
        {
            if (ModelState.IsValid)
            {
                thread.StartDate = DateTime.Now;
                _context.Add(thread);
                await _context.SaveChangesAsync();
                return RedirectToAction(thread.Section);
            }
            return View(thread);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("Id,ThreadId,Content,Author")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Date = DateTime.Now;
                string section = _context.Thread.FirstOrDefault(u => u.Id == comment.ThreadId).Section;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return OpenSection(comment.ThreadId, section);
            }
            return View(comment);
        }
        public IActionResult OpenSection(int id, string section)
        {
            if (id == 0)
            {
                Threads = _context.Thread.Where(u => u.Section == section);
                return View("Threads", Threads);
            }
            else
            {
                ThreadMultipleModel ThreadMultipleModel = new ThreadMultipleModel
                {
                    Thread = _context.Thread.Where(u => u.Section == section).FirstOrDefault(y => y.Id == id),
                    Comments = _context.Comment.Where(u => u.ThreadId == id)
                };
                ThreadMultipleModel.Thread.CreatorImage = _context.Users.FirstOrDefault(u => u.UserName == ThreadMultipleModel.Thread.Creator).ImageName;
                foreach (var item in ThreadMultipleModel.Comments)
                {
                    item.AuthorImage = _context.Users.FirstOrDefault(u => u.UserName == item.Author).ImageName;
                }
                ThreadMultipleModel.Comment = new Comment { ThreadId = id };
                return View("Thread", ThreadMultipleModel);
            }
        }
    }

}

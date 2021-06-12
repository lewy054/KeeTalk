using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KeeTalk.Data;
using KeeTalk.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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
            string section = referer.Split('=').Last();
            Thread Thread = new Thread { Section = section };
            return View(Thread);
        }

        // POST: Threads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Creator,Section")] Thread thread)
        {
            {
                if (ModelState.IsValid)
                {
                    thread.StartDate = DateTime.Now;
                    _context.Add(thread);
                    await _context.SaveChangesAsync();
                    //redirect needed
                    return Section(0, thread.Section);
                }
                return View(thread);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateComment([Bind("Id,ThreadId,Content,Author")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Date = DateTime.Now;
                string section = _context.Thread.FirstOrDefault(u => u.Id == comment.ThreadId).Section;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return Section(comment.ThreadId, section);
            }
            return View(comment);
        }
        public IActionResult Section(int id, string section)
        {
            if (id == 0)
            {
                //read one thread
                int maxLength = 200;
                Threads = _context.Thread.Where(u => u.Section == section);
                foreach (var item in Threads)
                {
                    item.Content = item.Content.Substring(0, Math.Min(item.Content.Length, maxLength)) + "...";
                    var lastComment = _context.Comment.OrderByDescending(s => s.Id).FirstOrDefault(s => s.ThreadId == item.Id);
                    if (lastComment == null)
                    {
                        //there is no comments, get author post
                        item.LastCommentAuthor = item.Creator;
                        item.LastCommentAuthorImage = _context.Users.FirstOrDefault(u => u.UserName == item.Creator).ImageName;
                        item.LastCommentDate = item.StartDate;
                    }
                    else
                    {
                        //get data from last comment
                        item.LastCommentAuthor = lastComment.Author;
                        item.LastCommentAuthorImage = _context.Users.FirstOrDefault(u => u.UserName == lastComment.Author).ImageName;
                        item.LastCommentDate = lastComment.Date;
                    }
                }
                return View("Threads", Threads);
            }
            else
            {
                //all threads in section
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

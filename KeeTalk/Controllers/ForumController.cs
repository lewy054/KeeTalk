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
        private readonly UserManager<ApplicationUser> _userManager;
        public Thread Thread { get; set; }
        public IEnumerable<Thread> Threads { get; set; }
        public ForumController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Author,Section")] Thread thread)
        {
            {
                if (ModelState.IsValid)
                {
                    thread.AuthorId = _userManager.GetUserId(User);
                    thread.StartDate = DateTime.Now;
                    _context.Add(thread);
                    await _context.SaveChangesAsync();
                    return Redirect($"Section?section={thread.Section}");
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
                comment.AuthorId = _userManager.GetUserId(User);
                string section = _context.Thread.FirstOrDefault(u => u.Id == comment.ThreadId).Section;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return Redirect($"Section/{comment.ThreadId}?section={section}");
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
                    int length = Math.Min(item.Content.Length, maxLength);
                    if (item.Content.Length > length)
                    {
                        item.Content = item.Content.Substring(0, length) + "...";
                    }
                    item.AuthorName = _userManager.Users.Where(u => u.Id == item.AuthorId).FirstOrDefault().UserName;

                    var lastComment = _context.Comment.OrderByDescending(s => s.Id).FirstOrDefault(s => s.ThreadId == item.Id);
                    if (lastComment == null)
                    {
                        //there is no comments, get author post
                        item.LastCommentAuthor = item.AuthorName;
                        item.LastCommentAuthorImage = _context.Users.FirstOrDefault(u => u.UserName == item.AuthorName).ImageName;
                        item.LastCommentDate = item.StartDate;
                    }
                    else
                    {
                        //get data from last comment
                        item.LastCommentAuthor = _userManager.Users.Where(u => u.Id == lastComment.AuthorId).FirstOrDefault().UserName;
                        item.LastCommentAuthorImage = _context.Users.FirstOrDefault(u => u.UserName == item.LastCommentAuthor).ImageName;
                        item.LastCommentDate = lastComment.Date;
                    }
                }
                return View("Threads", Threads);
            }
            else
            {
                //all threads in section
                Thread = _context.Thread.Where(u => u.Section == section).FirstOrDefault(y => y.Id == id);
                Thread.AuthorName = _userManager.Users.Where(u => u.Id == Thread.AuthorId).FirstOrDefault().UserName;
                Thread.AuthorImage = _context.Users.FirstOrDefault(u => u.UserName == Thread.AuthorName).ImageName;
                ThreadMultipleModel ThreadMultipleModel = new ThreadMultipleModel
                {
                    Comments = _context.Comment.Where(u => u.ThreadId == id),
                    Thread = Thread
                };

                foreach (var item in ThreadMultipleModel.Comments)
                {
                    item.AuthorName = _userManager.Users.Where(u => u.Id == item.AuthorId).FirstOrDefault().UserName;
                    item.AuthorImage = _context.Users.FirstOrDefault(u => u.UserName == item.AuthorName).ImageName;
                }
                ThreadMultipleModel.Comment = new Comment { ThreadId = id };
                return View("Thread", ThreadMultipleModel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteThread(int id)
        {
            var thread = _context.Thread.Where(u => u.Id == id).FirstOrDefault();
            var comments = _context.Comment.Where(u => u.ThreadId == id).ToList();
            foreach (var comment in comments)
            {
                _context.Comment.Remove(comment);
            }
            _context.Thread.Remove(thread);
            _context.SaveChanges();
            return Redirect($"/Forum/Section?section={thread.Section}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteComment(int id)
        {
            var comment = _context.Comment.Where(u => u.Id == id).FirstOrDefault();
            var section = _context.Thread.Where(u => u.Id == comment.ThreadId).FirstOrDefault().Section;
            _context.Comment.Remove(comment);
            _context.SaveChanges();
            return Redirect($"/Forum/Section/{comment.ThreadId}?section={section}");
        }

        // GET: ChatRoom/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditComment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: ChatRoom/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditComment(int id, [Bind("Id,ThreadId,AuthorId,Content,Date")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                comment.EditDate = DateTime.Now;
                comment.EditedBy = User.Identity.Name;
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var section = _context.Thread.Where(u => u.Id == comment.ThreadId).FirstOrDefault().Section;
                return Redirect($"/Forum/Section/{comment.ThreadId}?section={section}");
            }
            return View(comment);

        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}

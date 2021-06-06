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
        public IEnumerable<Thread> Thread { get; set; }
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
            string section = baseUri.Segments[baseUri.Segments.Length - 1];
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

        public IActionResult News()
        {
            Thread = _context.Thread.Where(u => u.Section == "News");
            return View("Threads", Thread);
        }
        public IActionResult Administration()
        {
            Thread = _context.Thread.Where(u => u.Section == "Administration");
            return View("Threads", Thread);
        }
        public IActionResult ReportPlayer()
        {
            Thread = _context.Thread.Where(u => u.Section == "ReportPlayer");
            return View("Threads", Thread);
        }
        public IActionResult ReportAdmin()
        {
            Thread = _context.Thread.Where(u => u.Section == "ReportAdmin");
            return View("Threads", Thread);
        }
        public IActionResult ReportBug()
        {
            Thread = _context.Thread.Where(u => u.Section == "ReportBug");
            return View("Threads", Thread);
        }
        public IActionResult Playground()
        {
            Thread = _context.Thread.Where(u => u.Section == "Playground");
            return View("Threads", Thread);
        }
    }
}

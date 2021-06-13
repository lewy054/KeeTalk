using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KeeTalk.Data;
using KeeTalk.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KeeTalk.Controllers
{
    [Authorize]
    public class ChatListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;


        public ChatListController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        // GET: ChatRoom
        public async Task<IActionResult> Index()
        {
            List<ChatRoom> chatRoomList = new List<ChatRoom>();
            chatRoomList = await _context.ChatRoom.ToListAsync();
            foreach (var room in chatRoomList)
            {
                room.AuthorName = _userManager.Users.Where(y => y.Id == room.AuthorId).FirstOrDefault().UserName;
            }
            IEnumerable<ChatRoom> chatRoom = chatRoomList;
            return View(chatRoom);
        }

        // GET: ChatRoom/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRoom = await _context.ChatRoom.FirstOrDefaultAsync(m => m.Id == id);

            if (chatRoom == null)
            {
                return NotFound();
            }
            if (chatRoom.AuthorId == _userManager.GetUserId(User))
            {
                return View(chatRoom);
            }
            return Unauthorized();
        }

        // GET: ChatRoom/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatRoom/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AuthorId,Name,Description,Color,ImageFile")] ChatRoom chatRoom)
        {
            if (ModelState.IsValid)
            {
                chatRoom.AuthorId = _userManager.GetUserId(User);
                if (chatRoom.ImageFile == null || chatRoom.ImageFile.Length == 0)
                {
                    chatRoom.ImageName = "default.png";
                }
                else
                {
                    // save image
                    string fileName = Path.GetFileNameWithoutExtension(chatRoom.ImageFile.FileName).Trim();
                    string extension = Path.GetExtension(chatRoom.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    chatRoom.ImageName = fileName;
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string path = Path.Combine(wwwRootPath + "/Images/roomImages/", fileName);
                    using var fileStream = new FileStream(path, FileMode.Create);
                    await chatRoom.ImageFile.CopyToAsync(fileStream);
                }
                _context.Add(chatRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatRoom);
        }

        // GET: ChatRoom/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatRoom = await _context.ChatRoom.FindAsync(id);

            if (chatRoom == null)
            {
                return NotFound();
            }
            if (chatRoom.AuthorId == _userManager.GetUserId(User))
            {
                return View(chatRoom);
            }
            return Unauthorized();
        }

        // POST: ChatRoom/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Color")] ChatRoom chatRoom)
        {
            if (id != chatRoom.Id)
            {
                return NotFound();
            }
            if (chatRoom.AuthorId == _userManager.GetUserId(User))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(chatRoom);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ChatRoomExists(chatRoom.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(chatRoom);
            }
            return Unauthorized();
        }

        // GET: ChatRoom/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var chatRoom = await _context.ChatRoom.FirstOrDefaultAsync(m => m.Id == id);
            if (chatRoom == null)
            {
                return NotFound();
            }
            if (chatRoom.AuthorId == _userManager.GetUserId(User))
            {
                return View(chatRoom);
            }
            return Unauthorized();
        }

        // POST: ChatRoom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chatRoom = await _context.ChatRoom.FindAsync(id);
            if (chatRoom.AuthorId == _userManager.GetUserId(User))
            {
                if (chatRoom.ImageName != "default.png")
                {
                    // delete image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string path = Path.Combine(wwwRootPath + "/Images/roomImages", chatRoom.ImageName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    // delete room
                    _context.ChatRoom.Remove(chatRoom);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return Unauthorized();
        }

        private bool ChatRoomExists(int id)
        {
            return _context.ChatRoom.Any(e => e.Id == id);
        }
    }
}

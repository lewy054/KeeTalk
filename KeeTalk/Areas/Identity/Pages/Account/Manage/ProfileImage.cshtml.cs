using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KeeTalk.Data;
using KeeTalk.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
namespace KeeTalk.Areas.Identity.Pages.Account.Manage
{
    public class ProfileImageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ProfileImageModel> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _context;

        public ProfileImageModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ProfileImageModel> logger,
            IWebHostEnvironment hostEnvironment,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Column(TypeName = "nvarchar(100)")]
            [DisplayName("Image Name")]
            public string ImageName { get; set; }
            [NotMapped]
            [DisplayName("Upload File")]
            public IFormFile ImageFile { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            string path = "";
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (!String.IsNullOrEmpty(user.ImageName))
            {
                //delete previous image
                path = Path.Combine(wwwRootPath + "/Images/profileImages/", user.ImageName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            // save new image
            string fileName = Path.GetFileNameWithoutExtension(Input.ImageFile.FileName).Trim();
            string extension = Path.GetExtension(Input.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            path = Path.Combine(wwwRootPath + "/Images/profileImages/", fileName);
            using var fileStream = new FileStream(path, FileMode.Create);
            await Input.ImageFile.CopyToAsync(fileStream);

            // add new image to db
            user.ImageName = fileName;
            await _context.SaveChangesAsync();

            _logger.LogInformation("User changed their image successfully.");
            StatusMessage = "Your image has been changed.";

            return RedirectToPage();
        }
    }
}

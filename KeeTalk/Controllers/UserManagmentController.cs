using KeeTalk.Data;
using KeeTalk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace KeeTalk.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagmentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserManagmentController> _logger;
        private readonly ApplicationDbContext _context;
        public UserManagmentController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UserManagmentController> logger,
            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            UserRolesMultiModel UserRolesMultiModel = new UserRolesMultiModel();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Banned = user.Banned,
                    BanEndTime = user.BanEndDate,
                    TotalBans = user.BanCount,
                    Email = user.Email,
                    Roles = await GetUserRoles(user)
                };
                userRolesViewModel.Add(thisViewModel);
            }
            UserRolesMultiModel.UserRolesViewModelList = userRolesViewModel;
            return View(UserRolesMultiModel);
        }
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(UserRolesMultiModel userRolesMultiModel)
        {
            var user = await _userManager.FindByIdAsync(userRolesMultiModel.UserRolesViewModel.UserId);
            if (user.Banned)
            {
                //unban
                user.Banned = false;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User {userRolesMultiModel.UserRolesViewModel.UserName} unbanned by {User.Identity.Name}");
            }
            else
            {
                //ban
                user.Banned = true;
                user.BanStartDate = DateTime.Now;
                user.BanEndDate = userRolesMultiModel.UserRolesViewModel.BanEndTime;
                user.BanCount += 1;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"User {userRolesMultiModel.UserRolesViewModel.UserId} banned by {User.Identity.Name} to {userRolesMultiModel.UserRolesViewModel.BanEndTime} ");
            }
            return RedirectToAction("Index");
        }
    }
}

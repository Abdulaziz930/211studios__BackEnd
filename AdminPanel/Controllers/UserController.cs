using AdminPanel.ViewModels;
using DataAccess.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace AdminPanel.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allUsers = await _userManager.Users.ToListAsync();
            ViewBag.PageCount = Decimal.Ceiling((decimal)allUsers.Count / 5);
            ViewBag.Page = page;

            if (allUsers.Count > 0 && (ViewBag.PageCount < page || page <= 0))
                return NotFound();


            var users = new List<UserViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userVM = new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Position = user.Position,
                    Image = user.Image,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                    IsActive = user.IsActive
                };

                users.Add(userVM);
            }

            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel createUserVM)
        {
            var dbUser = await _userManager.FindByEmailAsync(createUserVM.Email);
            if (dbUser != null)
            {
                ModelState.AddModelError("Email", "There is a user with this email!");
                return View();
            }

            if (createUserVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo field cannot be empty");
                return View();
            }

            if (!createUserVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "This is not a picture");
                return View();
            }

            if (!createUserVM.Photo.IsSizeAllowed(3000))
            {
                ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                return View();
            }

            var imageFolderPathList = new List<string>()
            {
                Constants.ImageFolderPath,
                Constants.FrontImageFolderPath
            };

            var imageFileName = await FileUtil.GenerateFileAsync(imageFolderPathList, createUserVM.Photo);

            var newUser = new AppUser
            {
                FullName = createUserVM.FullName,
                UserName = createUserVM.UserName,
                Email = createUserVM.Email,
                Position = createUserVM.Position,
                Image = imageFileName,
                IsActive = true
            };

            var identityResult = await _userManager.CreateAsync(newUser, createUserVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(newUser, RoleConstants.ModeratorRole);

            return RedirectToAction("Index");
        }
    }
}

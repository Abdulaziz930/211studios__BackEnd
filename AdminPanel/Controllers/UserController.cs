using AdminPanel.ViewModels;
using Business.Abstract;
using DataAccess.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace AdminPanel.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        #region Create

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

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(string id)
        {
            if (id is null)
                return BadRequest();

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            var user = new UpdateUserViewModel
            {
                FullName = dbUser.FullName,
                UserName = dbUser.UserName,
                Position = dbUser.Position,
                Email = dbUser.Email,
                Image = dbUser.Image,
                Description = dbUser.Description ?? ""
            };

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateUserViewModel updateUserVM)
        {
            if (id is null)
                return BadRequest();

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            var imageFileName = dbUser.Image;

            if (updateUserVM.Photo != null)
            {
                if (!updateUserVM.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "This is not a picture");
                    return View();
                }

                if (!updateUserVM.Photo.IsSizeAllowed(3000))
                {
                    ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                    return View();
                }

                var paths = new List<string>();

                var backPath = Path.Combine(Constants.ImageFolderPath, dbUser.Image);
                var frontPath = Path.Combine(Constants.FrontImageFolderPath, dbUser.Image);

                paths.Add(backPath);
                paths.Add(frontPath);

                foreach (var path in paths)
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                var imageFolderPathList = new List<string>()
                {
                    Constants.ImageFolderPath,
                    Constants.FrontImageFolderPath
                };

                imageFileName = await FileUtil.GenerateFileAsync(imageFolderPathList, updateUserVM.Photo);
            }

            if (!ModelState.IsValid)
            {
                return View(dbUser);
            }

            dbUser.FullName = updateUserVM.FullName;
            dbUser.UserName = updateUserVM.UserName;
            dbUser.Email = updateUserVM.Email;
            dbUser.Position = updateUserVM.Position;
            dbUser.Description = updateUserVM.Description;
            dbUser.Image = imageFileName;

            await _userManager.UpdateAsync(dbUser);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ChangePassword

        public async Task<IActionResult> ChangePassword(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var changePasswordViewModel = new ChangePasswordViewModel
            {
                FullName = user.FullName
            };

            return View(changePasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordViewModel changePasswordVM)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            var changePasswordViewModel = new ChangePasswordViewModel
            {
                FullName = dbUser.FullName
            };

            if (!ModelState.IsValid)
            {
                return View(changePasswordViewModel);
            }

            if (!await _userManager.CheckPasswordAsync(dbUser, changePasswordVM.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "Old password is not valid.");
                return View(changePasswordViewModel);
            }

            var result = await _userManager.ChangePasswordAsync(dbUser, changePasswordVM.OldPassword, changePasswordVM.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(changePasswordViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region ChangeRole

        public async Task<IActionResult> ChangeRole(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            var changeRoleViewModel = new ChangeRoleViewModel
            {
                FullName = dbUser.FullName,
                Role = (await _userManager.GetRolesAsync(dbUser)).FirstOrDefault(),
                Roles = GetRoles()
            };

            return View(changeRoleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id, ChangeRoleViewModel changeRoleVM, string role)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
                return NotFound();

            var changeRoleViewModel = new ChangeRoleViewModel
            {
                FullName = dbUser.FullName,
                Role = (await _userManager.GetRolesAsync(dbUser)).FirstOrDefault(),
                Roles = GetRoles()
            };

            if (!ModelState.IsValid)
            {
                return View(changeRoleViewModel);
            }

            string oldRole = (await _userManager.GetRolesAsync(dbUser)).FirstOrDefault();
            string newRole = changeRoleVM.Role;
            if (oldRole != newRole)
            {
                var addResult = await _userManager.AddToRoleAsync(dbUser, newRole);
                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError("", "Some problem exist");
                    return View(changeRoleViewModel);
                }

                var removeResult = await _userManager.RemoveFromRoleAsync(dbUser, oldRole);
                if (!removeResult.Succeeded)
                {
                    ModelState.AddModelError("", "Some problem exist");
                    return View(changeRoleViewModel);
                }
            }

            await _userManager.UpdateAsync(dbUser);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Activity

        public async Task<IActionResult> Activity(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            if (dbUser.IsActive)
            {
                dbUser.IsActive = false;
            }
            else
            {
                dbUser.IsActive = true;
            }

            await _userManager.UpdateAsync(dbUser);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            var userDetailVM = new UserDetailViewModel
            {
                Id = dbUser.Id,
                FullName = dbUser.FullName,
                UserName = dbUser.UserName,
                Email = dbUser.Email,
                Position = dbUser.Position,
                Description = dbUser.Description ?? "",
                Role = (await _userManager.GetRolesAsync(dbUser)).FirstOrDefault(),
                Image = dbUser.Image
            };

            return View(userDetailVM);
        }

        #endregion

        #region Login

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Dashboard");
            }

            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var existUser = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (existUser == null)
            {
                ModelState.AddModelError("", "Email or password is invalid.");
                return View();
            }

            if (existUser.IsActive == false)
            {
                ModelState.AddModelError("", "Your account is disabled.");
                return View();
            }

            var loginResult = await _signInManager.PasswordSignInAsync(existUser, loginViewModel.Password,
                loginViewModel.RememberMe, true);
            if (!loginResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is invalid.");
                return View();
            }

            return RedirectToAction(nameof(Index), "Dashboard");
        }

        #endregion

        #region Logout

        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return NotFound();
            }

            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region ResetPassword

        public IActionResult RedirectionToResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RedirectionToResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser is null)
                return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(dbUser);

            var link = Url.Action("ResetPassword", "User", new { dbUser.Id, token }, protocol: HttpContext.Request.Scheme);
            var title = "FORGOT YOUR PASSWORD?";
            var description = "Not to worry, we got you! <br /> Let’s get you a new password.";
            var buttonName = "Reset Password";
            var homeLink = Constants.AdminClientPort;
            var filePath = Constants.EmailFolderPath;
            var message = FileUtil.GetEmailView(filePath, link, title, description, buttonName, homeLink);
            await EmailUtil.SendEmailAsync(dbUser.Email, message, "ResetPassword");

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ResetPassword(string id, string token)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (string.IsNullOrEmpty(token))
                return BadRequest();

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string token, ResetPasswordViewModel passwordViewModel)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (string.IsNullOrEmpty(token))
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(passwordViewModel);
            }

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            var result = await _userManager.ResetPasswordAsync(dbUser, token, passwordViewModel.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(passwordViewModel);
            }

            return RedirectToAction("Login");
        }

        #endregion

        public List<string> GetRoles()
        {
            List<string> roles = new List<string>();

            roles.Add(RoleConstants.AdminRole);
            roles.Add(RoleConstants.ModeratorRole);

            return roles;
        }
    }
}

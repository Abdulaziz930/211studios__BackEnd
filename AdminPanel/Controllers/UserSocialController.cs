using AdminPanel.ViewModels;
using Business.Abstract;
using DataAccess.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = RoleConstants.AdminRole + "," + RoleConstants.ModeratorRole)]
    public class UserSocialController : Controller
    {
        private readonly IUserSocialService _userSocial;
        private readonly UserManager<AppUser> _userManager;

        public UserSocialController(IUserSocialService userSocial, UserManager<AppUser> userManager)
        {
            _userSocial = userSocial;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allStudios = await _userSocial.GetUserSocialsAsync();
            ViewBag.PageCount = Decimal.Ceiling((decimal)allStudios.Count / 5);
            ViewBag.Page = page;

            if (allStudios.Count > 0 && (ViewBag.PageCount < page || page <= 0))
                return NotFound();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            int skipCount = (page - 1) * 5;

            var userSocials = await _userSocial.GetUserSocialsAsync(user.Id, skipCount, 5);
            if (userSocials is null)
                return NotFound();

            var userSocialVMList = new List<UserSocialViewModel>();
            foreach (var userSocial in userSocials)
            {
                var userSocialVM = new UserSocialViewModel
                {
                    Id = userSocial.Id,
                    Link = userSocial.Link,
                    Icon = userSocial.Icon
                };
                userSocialVMList.Add(userSocialVM);
            }

            return View(userSocialVMList);
        }

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserSocialMedia userSocial)
        {
            if (!ModelState.IsValid)
            {
                return View(userSocial);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            userSocial.AppUserId = user.Id;

            List<UserSocialMedia> userSocials = new List<UserSocialMedia>();
            if(user.UserSocialMedias != null)
            {
                user.UserSocialMedias.Add(userSocial);
            }
            else
            {
                userSocials.Add(userSocial);
                user.UserSocialMedias = userSocials;
            }

            await _userSocial.AddUserSocialAsync(userSocial);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            var userSocial = await _userSocial.GetUserSocialAsync(id.Value);
            if (userSocial is null)
                return NotFound();

            if (userSocial.AppUserId != user.Id)
                return BadRequest();

            var userSocialVM = new UserSocialViewModel
            {
                Id = userSocial.Id,
                Link = userSocial.Link,
                Icon = userSocial.Icon
            };

            return View(userSocialVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, UserSocialViewModel userSocialVM)
        {
            if (id is null)
                return BadRequest();

            if (id != userSocialVM.Id)
                return BadRequest();

            var userSocial = await _userSocial.GetUserSocialAsync(id.Value);
            if (userSocial is null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View(userSocial);
            }

            userSocial.Link = userSocialVM.Link;
            userSocial.Icon = userSocial.Icon;

            await _userSocial.UpdateUserSocialAsync(userSocial);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            var userSocial = await _userSocial.GetUserSocialAsync(id.Value);
            if (userSocial is null)
                return NotFound();

            if (userSocial.AppUserId != user.Id)
                return BadRequest();

            var userSocialVM = new UserSocialViewModel
            {
                Id = userSocial.Id,
                Link = userSocial.Link,
                Icon = userSocial.Icon
            };

            return View(userSocialVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteSocial(int? id)
        {
            if (id is null)
                return BadRequest();

            var userSocial = await _userSocial.GetUserSocialAsync(id.Value);
            if (userSocial is null)
                return NotFound();

            userSocial.IsDeleted = true;

            await _userSocial.UpdateUserSocialAsync(userSocial);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            var userSocial = await _userSocial.GetUserSocialAsync(id.Value);
            if (userSocial is null)
                return NotFound();

            if (userSocial.AppUserId != user.Id)
                return BadRequest();

            var userSocialVM = new UserSocialViewModel
            {
                Id = userSocial.Id,
                Link = userSocial.Link,
                Icon = userSocial.Icon
            };

            return View(userSocialVM);
        }

        #endregion
    }
}

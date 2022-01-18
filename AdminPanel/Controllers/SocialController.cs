using AdminPanel.ViewModels;
using Business.Abstract;
using DataAccess.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = RoleConstants.AdminRole + "," + RoleConstants.ModeratorRole)]
    public class SocialController : Controller
    {
        private readonly ISocialService _socialService;

        public SocialController(ISocialService socialService)
        {
            _socialService = socialService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allSocials = await _socialService.GetSocialsAsync();
            ViewBag.PageCount = Decimal.Ceiling((decimal)allSocials.Count / 5);
            ViewBag.Page = page;

            if (allSocials.Count > 0 && (ViewBag.PageCount < page || page <= 0))
                return NotFound();

            int skipCount = (page - 1) * 5;

            var socials = await _socialService.GetSocialsAsync(skipCount, 5);
            if (socials is null)
                return NotFound();

            var socialsVM = new List<SocialViewModel>();
            foreach (var social in socials)
            {
                var socialVM = new SocialViewModel
                {
                    Id = social.Id,
                    SocialName = social.SocialName
                };
                socialsVM.Add(socialVM);
            }

            return View(socialsVM);
        }

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Social social)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isExist = await _socialService
                .CheckSocialAsync(x => (x.SocialName == social.SocialName || x.SocialLink == social.SocialLink) && x.IsDeleted == false);
            if (isExist)
            {
                ModelState.AddModelError("", "There is a social media with this name or link");
                return View();
            }

            await _socialService.AddAsync(social);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var social = await _socialService.GetSocialAsync(id.Value);
            if (social is null)
                return NotFound();

            var socialDetailVM = new SocialDetailViewModel
            {
                Id = social.Id,
                SocialName = social.SocialName,
                SocialLink = social.SocialLink,
                SocialIcon = social.SocialIcon
            };

            return View(socialDetailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, SocialDetailViewModel socialVM)
        {
            if (id is null)
                return BadRequest();

            if (id != socialVM.Id)
                return BadRequest();

            var social = await _socialService.GetSocialAsync(id.Value);
            if (social is null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View(socialVM);
            }

            var isExist = await _socialService
                .CheckSocialAsync(x => (x.SocialName == social.SocialName || x.SocialLink == social.SocialLink) && x.IsDeleted == false && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("", "There is a social media with this name or link");
                return View();
            }

            social.SocialName = socialVM.SocialName;
            social.SocialLink = socialVM.SocialLink;
            social.SocialIcon = socialVM.SocialIcon;

            await _socialService.UpdateAsync(social);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var social = await _socialService.GetSocialAsync(id.Value);
            if (social is null)
                return NotFound();

            var socialDetailVM = new SocialDetailViewModel
            {
                Id = social.Id,
                SocialName = social.SocialName,
                SocialLink = social.SocialLink,
                SocialIcon = social.SocialIcon
            };

            return View(socialDetailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteSocial(int? id)
        {
            if (id is null)
                return BadRequest();

            var social = await _socialService.GetSocialAsync(id.Value);
            if (social is null)
                return NotFound();

            social.IsDeleted = true;

            await _socialService.UpdateAsync(social);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var social = await _socialService.GetSocialAsync(id.Value);
            if (social is null)
                return NotFound();

            var socialDetailVM = new SocialDetailViewModel
            {
                Id = social.Id,
                SocialName = social.SocialName,
                SocialLink = social.SocialLink,
                SocialIcon = social.SocialIcon
            };

            return View(socialDetailVM);
        }

        #endregion
    }
}

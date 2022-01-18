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
using Utils;
using static Utils.CommonEnums;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = RoleConstants.AdminRole + "," + RoleConstants.ModeratorRole)]
    public class BannerController : Controller
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        public async Task<IActionResult> Index()
        {
            var banners = await _bannerService.GetAllBannersAsync();
            if (banners is null)
                return NotFound();

            var bannersVM = new List<BannerViewModel>();
            foreach (var banner in banners)
            {
                var bannerVM = new BannerViewModel
                {
                    Id = banner.Id,
                    Title = banner.Title,
                    Image = banner.Image,
                    PageName = banner.Menu.PageName
                };
                bannersVM.Add(bannerVM);
            }

            return View(bannersVM);
        }

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var banner = await _bannerService.GetBannerWithIncludeAsync(id.Value);
            if (banner is null)
                return NotFound();

            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Banner banner)
        {
            if (id == null)
                return BadRequest();

            if (id != banner.Id)
                return BadRequest();

            var dbBanner = await _bannerService.GetBannerWithIncludeAsync(id.Value);
            if (dbBanner is null)
                return NotFound();

            var isExists = await _bannerService.CheckBannerAsync(x => x.IsDeleted == false
                                                && x.Title.ToLower() == banner.Title.ToLower() && x.Id != dbBanner.Id);
            if (isExists)
            {
                ModelState.AddModelError("Title", "There is a banner title with this name");
                return View(dbBanner);
            }

            var imageFileName = dbBanner.Image;

            if (banner.Photo != null)
            {
                if (!banner.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "This is not a picture");
                    return View(dbBanner);
                }

                if (!banner.Photo.IsSizeAllowed(3000))
                {
                    ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                    return View(dbBanner);
                }

                imageFileName = await FileUtil.UpdateFileAsync(dbBanner.Image, Constants.ImageFolderPath, banner.Photo, FileType.Image);
            }

            if (!ModelState.IsValid)
            {
                return View(dbBanner);
            }

            dbBanner.Title = banner.Title;
            dbBanner.Description = banner.Description;
            dbBanner.Image = imageFileName;
            dbBanner.LastModificationDate = DateTime.UtcNow;

            await _bannerService.UpdateAsync(dbBanner);

            return RedirectToAction("Index");
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var banner = await _bannerService.GetBannerWithIncludeAsync(id.Value);
            if (banner is null)
                return NotFound();

            var bannerVM = new BannerDetailViewModel
            {
                Id = banner.Id,
                Title = banner.Title,
                Description = banner.Description,
                Image = banner.Image,
                PageName = banner.Menu.PageName,
                LastModificationDate = banner.LastModificationDate
            };

            return View(bannerVM);
        }

        #endregion
    }
}

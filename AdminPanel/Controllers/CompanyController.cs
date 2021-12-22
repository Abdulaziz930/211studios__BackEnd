using AdminPanel.ViewModels;
using Business.Abstract;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;
using static Utils.CommonEnums;

namespace AdminPanel.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IStudioService _studioService;

        public CompanyController(IStudioService studioService)
        {
            _studioService = studioService;
        }

        public async Task<IActionResult> Index()
        {
            var studios = await _studioService.GetStudiosAsync();
            if (studios is null)
                return NotFound();

            var studio = studios.FirstOrDefault();

            var studioVM = new StudioViewModel
            {
                Id = studio.Id,
                Title = studio.Title,
                Image = studio.Image
            };

            return View(studioVM);
        }

        #region Create

        public async Task<IActionResult> Create()
        {
            var studios = await _studioService.GetStudiosAsync();
            if (studios is null)
                return NotFound();

            if (studios.Count == 1)
                return BadRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Studio studio)
        {
            if (studio.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo field cannot be empty");
                return View();
            }

            if (!studio.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "This is not a picture");
                return View();
            }

            if (!studio.Photo.IsSizeAllowed(3000))
            {
                ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                return View();
            }

            if (studio.Banner.Photo == null)
            {
                ModelState.AddModelError("Banner.Photo", "Photo field cannot be empty");
                return View();
            }

            if (!studio.Banner.Photo.IsImage())
            {
                ModelState.AddModelError("Banner.Photo", "This is not a picture");
                return View();
            }

            var imageFileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, studio.Photo, FileType.Image);
            var bannerImageFileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, studio.Banner.Photo, FileType.Image);

            studio.Image = imageFileName;
            studio.Banner.Image = bannerImageFileName;

            if (!ModelState.IsValid)
            {
                return View(studio);
            }


            return RedirectToAction("Index");
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var studio = await _studioService.GetStudioWithIncludeAsync(id.Value);
            if (studio is null)
                return NotFound();

            return View(studio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Studio studio)
        {
            if (id is null)
                return BadRequest();

            if (id != studio.Id)
                return BadRequest();

            var dbStudio = await _studioService.GetStudioWithIncludeAsync(id.Value);
            if (dbStudio is null)
                return NotFound();

            var imageFileName = dbStudio.Image;
            var bannerImageFileName = dbStudio.Banner.Image;

            if (studio.Photo != null)
            {
                if (!studio.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "This is not a picture");
                    return View(dbStudio);
                }

                if (!studio.Photo.IsSizeAllowed(3000))
                {
                    ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                    return View(dbStudio);
                }

                imageFileName = await FileUtil.UpdateFileAsync(dbStudio.Image, Constants.ImageFolderPath, studio.Photo, FileType.Image);
            }

            if (studio.Banner.Photo != null)
            {
                if (!studio.Banner.Photo.IsImage())
                {
                    ModelState.AddModelError("Banner.Photo", "This is not a picture");
                    return View(dbStudio);
                }

                bannerImageFileName = await FileUtil.UpdateFileAsync(dbStudio.Banner.Image, Constants.ImageFolderPath
                    , studio.Banner.Photo, FileType.Image);
            }

            if (!ModelState.IsValid)
            {
                return View(dbStudio);
            }

            dbStudio.Title = studio.Title;
            dbStudio.Description = studio.Description;
            dbStudio.Image = imageFileName;
            dbStudio.Banner.Image = bannerImageFileName;
            dbStudio.Banner.Title = studio.Banner.Title;
            dbStudio.Banner.Description = studio.Banner.Description;

            await _studioService.UpdateAsync(dbStudio);

            return RedirectToAction("Index");
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var studio = await _studioService.GetStudioWithIncludeAsync(id.Value);
            if (studio is null)
                return NotFound();

            var studioDetailVM = new StudioDetailViewModel
            {
                Id = studio.Id,
                Title = studio.Title,
                Description = studio.Description,
                Image = studio.Image,
                BannerDescription = studio.Banner.Description,
                BannerTitle = studio.Banner.Title,
                BannerImage = studio.Banner.Image
            };

            return View(studioDetailVM);
        }

        #endregion
    }
}

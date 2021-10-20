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

        public IActionResult Create()
        {
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

            var imageFolderPathList = new List<string>()
            {
                Constants.ImageFolderPath,
                Constants.FrontImageFolderPath
            };

            var bannerImageFolderPathList = new List<string>()
            {
                Constants.ImageFolderPath,
                Constants.FrontImageFolderPath
            };

            var imageFileName = await FileUtil.GenerateFileAsync(imageFolderPathList, studio.Photo);
            var bannerImageFileName = await FileUtil.GenerateFileAsync(bannerImageFolderPathList, studio.Banner.Photo);

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

                var paths = new List<string>();

                var backPath = Path.Combine(Constants.ImageFolderPath, dbStudio.Image);
                var frontPath = Path.Combine(Constants.FrontImageFolderPath, dbStudio.Image);

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

                imageFileName = await FileUtil.GenerateFileAsync(imageFolderPathList, studio.Photo);
            }

            if (studio.Banner.Photo != null)
            {
                if (!studio.Banner.Photo.IsImage())
                {
                    ModelState.AddModelError("Banner.Photo", "This is not a picture");
                    return View(dbStudio);
                }

                var paths = new List<string>();

                var backPath = Path.Combine(Constants.ImageFolderPath, dbStudio.Image);
                var frontPath = Path.Combine(Constants.FrontImageFolderPath, dbStudio.Image);

                paths.Add(backPath);
                paths.Add(frontPath);

                foreach (var path in paths)
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                var bannerImageFolderPathList = new List<string>()
                {
                    Constants.ImageFolderPath,
                    Constants.FrontImageFolderPath
                };

                bannerImageFileName = await FileUtil.GenerateFileAsync(bannerImageFolderPathList, studio.Banner.Photo);
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

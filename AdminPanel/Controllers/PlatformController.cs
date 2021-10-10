﻿using AdminPanel.ViewModels;
using Business.Abstract;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Controllers
{
    public class PlatformController : Controller
    {
        private readonly IPlatformService _platformService;

        public PlatformController(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allPlatforms = await _platformService.GetPlatformsAsync();
            ViewBag.PageCount = Decimal.Ceiling((decimal)allPlatforms.Count / 5);
            ViewBag.Page = page;

            if (allPlatforms.Count > 0 && (ViewBag.PageCount < page || page <= 0))
                return NotFound();

            int skipCount = (page - 1) * 5;

            var platforms = await _platformService.GetPlatformsAsync(skipCount, 5);
            if (platforms is null)
                return NotFound();

            var platformsVM = new List<PlatformViewModel>();
            foreach (var platform in platforms)
            {
                var platformVM = new PlatformViewModel
                {
                    Id = platform.Id,
                    Name = platform.Name
                };
                platformsVM.Add(platformVM);
            }

            return View(platformsVM);
        }

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Platform platform)
        {
            if (!ModelState.IsValid)
            {
                return View(platform);
            }

            await _platformService.AddAsync(platform);

            return RedirectToAction("Index");
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var platform = await _platformService.GetPlatformAsync(id.Value);
            if (platform is null)
                return NotFound();

            var platformDetailVM = new PlatformDetailViewModel
            {
                Id = platform.Id,
                Name = platform.Name,
                Logo = platform.Logo
            };

            return View(platformDetailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Platform platform)
        {
            if (id is null)
                return BadRequest();

            if (id != platform.Id)
                return BadRequest();

            var dbPlatform = await _platformService.GetPlatformAsync(id.Value);
            if (dbPlatform is null)
                return NotFound();

            dbPlatform.Name = platform.Name;
            dbPlatform.Logo = platform.Logo;

            await _platformService.UpdateAsync(dbPlatform);

            return RedirectToAction("Index");
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var allPlatforms = await _platformService.GetPlatformsAsync();
            if (allPlatforms.Count < 2)
            {
                return BadRequest();
            }

            var platform = await _platformService.GetPlatformAsync(id.Value);
            if (platform is null)
                return NotFound();

            var platformDetailVM = new PlatformDetailViewModel
            {
                Id = platform.Id,
                Name = platform.Name,
                Logo = platform.Logo
            };

            return View(platformDetailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePlatform(int? id)
        {
            if (id is null)
                return BadRequest();

            var platform = await _platformService.GetPlatformAsync(id.Value);
            if (platform is null)
                return NotFound();

            platform.IsDeleted = true;

            await _platformService.UpdateAsync(platform);

            return RedirectToAction("Index");
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var platform = await _platformService.GetPlatformAsync(id.Value);
            if (platform is null)
                return NotFound();

            var platformDetailVM = new PlatformDetailViewModel
            {
                Id = platform.Id,
                Name = platform.Name,
                Logo = platform.Logo
            };

            return View(platformDetailVM);
        }

        #endregion
    }
}

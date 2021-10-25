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
    public class TeamMemberBannerController : Controller
    {
        private readonly ITeamMemberBannerService _teamMemberBannerService;

        public TeamMemberBannerController(ITeamMemberBannerService teamMemberBannerService)
        {
            _teamMemberBannerService = teamMemberBannerService;
        }

        public async Task<IActionResult> Index()
        {
            var teamMemberBanners = await _teamMemberBannerService.GetTeamMembersAsync();
            if (teamMemberBanners is null)
                return NotFound();

            var teamMemberBannerListViewModel = new List<TeamMemberBannerViewModel>();
            foreach (var teamMemberBanner in teamMemberBanners)
            {
                var teamMemberBannerViewModel = new TeamMemberBannerViewModel
                {
                    Id = teamMemberBanner.Id,
                    Title = teamMemberBanner.Title,
                    Image = teamMemberBanner.Image
                };
                teamMemberBannerListViewModel.Add(teamMemberBannerViewModel);
            }

            return View(teamMemberBannerListViewModel);
        }

        #region Create

        public async Task<IActionResult> Create()
        {
            var teamMemberBanners = await _teamMemberBannerService.GetTeamMembersAsync();
            if (teamMemberBanners is null)
                return NotFound();

            if (teamMemberBanners.Count == 1)
                return BadRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMemberBanner teamMemberBanner)
        {
            if (teamMemberBanner.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo field cannot be empty");
                return View();
            }

            if (!teamMemberBanner.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "This is not a picture");
                return View();
            }

            if (!teamMemberBanner.Photo.IsSizeAllowed(3000))
            {
                ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                return View();
            }

            var imageFolderPathList = new List<string>()
            {
                Constants.ImageFolderPath,
                Constants.FrontImageFolderPath
            };

            var imageFileName = await FileUtil.GenerateFileAsync(imageFolderPathList, teamMemberBanner.Photo);

            teamMemberBanner.Image = imageFileName;

            if (!ModelState.IsValid)
            {
                return View(teamMemberBanner);
            }

            await _teamMemberBannerService.AddAsync(teamMemberBanner);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var teamMemberBanner = await _teamMemberBannerService.GetTeamMemberBannerAsync(id.Value);
            if (teamMemberBanner is null)
                return NotFound();

            return View(teamMemberBanner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, TeamMemberBanner teamMemberBanner)
        {
            if (id is null)
                return BadRequest();

            var dbTeamMemberBanner = await _teamMemberBannerService.GetTeamMemberBannerAsync(id.Value);
            if (dbTeamMemberBanner is null)
                return NotFound();

            var imageFileName = dbTeamMemberBanner.Image;

            if (teamMemberBanner.Photo != null)
            {
                if (!teamMemberBanner.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "This is not a picture");
                    return View(dbTeamMemberBanner);
                }

                if (!teamMemberBanner.Photo.IsSizeAllowed(3000))
                {
                    ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                    return View(dbTeamMemberBanner);
                }

                var paths = new List<string>();

                var backPath = Path.Combine(Constants.ImageFolderPath, dbTeamMemberBanner.Image);
                var frontPath = Path.Combine(Constants.FrontImageFolderPath, dbTeamMemberBanner.Image);

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

                imageFileName = await FileUtil.GenerateFileAsync(imageFolderPathList, teamMemberBanner.Photo);
            }

            if (!ModelState.IsValid)
            {
                return View(dbTeamMemberBanner);
            }

            dbTeamMemberBanner.Title = teamMemberBanner.Title;
            dbTeamMemberBanner.Description = teamMemberBanner.Description;
            dbTeamMemberBanner.Image = imageFileName;

            await _teamMemberBannerService.UpdateAsync(dbTeamMemberBanner);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var teamMemberBanner = await _teamMemberBannerService.GetTeamMemberBannerAsync(id.Value);
            if (teamMemberBanner is null)
                return NotFound();

            var teamMemberBannerDetailVM = new TeamMemberBannerDetailViewModel
            {
                Id = teamMemberBanner.Id,
                Title = teamMemberBanner.Title,
                Description = teamMemberBanner.Description,
                Image = teamMemberBanner.Image
            };

            return View(teamMemberBannerDetailVM);
        }

        #endregion
    }
}

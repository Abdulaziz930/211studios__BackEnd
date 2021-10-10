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
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ICategoryService _categoryService;

        public GameController(IGameService gameService, ICategoryService categoryService)
        {
            _gameService = gameService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allGames = await _gameService.GetGamesAsync();
            ViewBag.PageCount = Decimal.Ceiling((decimal)allGames.Count / 5);
            ViewBag.Page = page;

            if (allGames.Count > 0 && (ViewBag.PageCount < page || page <= 0))
                return NotFound();

            int skipCount = (page - 1) * 5;

            var games = await _gameService.GetGamesAsync(skipCount, 5);
            if (games is null)
                return NotFound();

            var gamesVM = new List<GameViewModel>();
            foreach (var game in games)
            {
                var gameVM = new GameViewModel
                {
                    Id = game.Id,
                    Name = game.Name,
                    Image = game.Image,
                    CategoryName = game.GameCategories.FirstOrDefault(x => x.Category.IsDeleted == false).Category.Name
                };
                gamesVM.Add(gameVM);
            }

            return View(gamesVM);
        }

        #region Create

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game game, List<int> categoriesId)
        {
            var categories = await _categoryService.GetCategoriesAsync();
            ViewBag.Categories = categories;

            if (game.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo field cannot be empty");
                return View();
            }

            if (game.GameDetail.VideoFile == null)
            {
                ModelState.AddModelError("GameDetail.VideoFile", "Photo field cannot be empty");
                return View();
            }

            if (!game.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "This is not a picture");
                return View();
            }

            if (!game.GameDetail.VideoFile.IsVideo())
            {
                ModelState.AddModelError("GameDetail.VideoFile", "This is not a video");
                return View();
            }

            if (!game.Photo.IsSizeAllowed(3000))
            {
                ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                return View();
            }

            var imageFileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, game.Photo);
            var videoFileName = await FileUtil.GenerateFileAsync(Constants.VideoFolderPath, game.GameDetail.VideoFile);

            game.Image = imageFileName;
            game.GameDetail.Video = videoFileName;

            if (!ModelState.IsValid)
            {
                return View(game);
            }

            if (categoriesId.Count == 0 || categoriesId == null)
            {
                ModelState.AddModelError("", "Please select category.");
                return View();
            }

            foreach (var item in categoriesId)
            {
                if (categories.All(x => x.Id != item))
                    return BadRequest();
            }

            var gameCategoryList = new List<GameCategory>();
            foreach (var item in categoriesId)
            {
                var gameCategory = new GameCategory
                {
                    CategoryId = item,
                    GameId = game.Id
                };
                gameCategoryList.Add(gameCategory);
            }
            game.GameCategories = gameCategoryList;

            game.GameDetail.CreationDate = DateTime.UtcNow;
            game.GameDetail.LastModificationDate = DateTime.UtcNow;

            await _gameService.AddRangeAsync(game, game.GameDetail);

            return RedirectToAction("Index");
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var categories = await _categoryService.GetCategoriesAsync();
            ViewBag.Categories = categories;

            var game = await _gameService.GetGameWithIncludeAsync(id.Value);
            if (game is null)
                return NotFound();

            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Game game, int? id, List<int> categoriesId)
        {
            if (id == null)
                return BadRequest();

            if (id != game.Id)
                return BadRequest();

            var categories = await _categoryService.GetCategoriesAsync();
            ViewBag.Categories = categories;

            var dbGame = await _gameService.GetGameWithIncludeAsync(id.Value);
            if (dbGame is null)
                return NotFound();

            var imageFileName = dbGame.Image;
            var videoFileName = dbGame.GameDetail.Video;

            if (game.Photo != null)
            {
                if (!game.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "This is not a picture");
                    return View(dbGame);
                }

                if (!game.Photo.IsSizeAllowed(3000))
                {
                    ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                    return View(dbGame);
                }

                var path = Path.Combine(Constants.ImageFolderPath, dbGame.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                imageFileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, game.Photo);
            }

            if (game.GameDetail.VideoFile != null)
            {
                if (!game.GameDetail.VideoFile.IsVideo())
                {
                    ModelState.AddModelError("GameDetail.VideoFile", "This is not a picture");
                    return View(dbGame);
                }

                var path = Path.Combine(Constants.VideoFolderPath, dbGame.GameDetail.Video);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                videoFileName = await FileUtil.GenerateFileAsync(Constants.VideoFolderPath, game.GameDetail.VideoFile);
            }

            if (!ModelState.IsValid)
            {
                return View(dbGame);
            }

            if (categoriesId.Count == 0 || categoriesId == null)
            {
                ModelState.AddModelError("", "Please select category.");
                return View(dbGame);
            }

            foreach (var item in categoriesId)
            {
                if (categories.All(x => x.Id != item))
                    return BadRequest();
            }

            var gameCategoryList = new List<GameCategory>();
            foreach (var item in categoriesId)
            {
                var gameCategory = new GameCategory
                {
                    CategoryId = item,
                    GameId = game.Id
                };
                gameCategoryList.Add(gameCategory);
            }
            dbGame.GameCategories = gameCategoryList;

            dbGame.GameDetail.LastModificationDate = DateTime.UtcNow;
            dbGame.Name = game.Name;
            dbGame.Description = game.Description;
            dbGame.Image = imageFileName;
            dbGame.GameDetail.GameLink = game.GameDetail.GameLink;
            dbGame.GameDetail.Size = game.GameDetail.Size;
            dbGame.GameDetail.Platform = game.GameDetail.Platform;
            dbGame.GameDetail.Video = videoFileName;

            await _gameService.UpdateAsync(dbGame);

            return RedirectToAction("Index");
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var game = await _gameService.GetGameWithIncludeAsync(id.Value);
            if (game is null)
                return NotFound();

            var categories = new List<string>();
            foreach (var category in game.GameCategories)
            {
                if (category.Category.IsDeleted == false)
                {
                    categories.Add(category.Category.Name);
                }
            }

            var gameDetailVM = new GameDetailViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                Image = game.Image,
                Video = game.GameDetail.Video,
                Size = game.GameDetail.Size,
                Platform = game.GameDetail.Platform,
                GameLink = game.GameDetail.GameLink,
                CreationDate = game.GameDetail.CreationDate,
                LastModificationDate = game.GameDetail.LastModificationDate,
                Categories = categories
            };

            return View(gameDetailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteGame(int? id)
        {
            if (id is null)
                return BadRequest();

            var game = await _gameService.GetGameWithIncludeAsync(id.Value);
            if (game is null)
                return NotFound();

            game.IsDeleted = true;

            var imagePath = Path.Combine(Constants.ImageFolderPath, game.Image);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            var videoPath = Path.Combine(Constants.VideoFolderPath, game.GameDetail.Video);

            if (System.IO.File.Exists(videoPath))
            {
                System.IO.File.Delete(videoPath);
            }

            await _gameService.UpdateAsync(game);

            return RedirectToAction("Index");
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var game = await _gameService.GetGameWithIncludeAsync(id.Value);
            if (game is null)
                return NotFound();

            var categories = new List<string>();
            foreach (var category in game.GameCategories)
            {
                if (category.Category.IsDeleted == false)
                {
                    categories.Add(category.Category.Name);
                }
            }

            var gameDetailVM = new GameDetailViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                Image = game.Image,
                Video = game.GameDetail.Video,
                Size = game.GameDetail.Size,
                Platform = game.GameDetail.Platform,
                GameLink = game.GameDetail.GameLink,
                CreationDate = game.GameDetail.CreationDate,
                LastModificationDate = game.GameDetail.LastModificationDate,
                Categories = categories
            };

            return View(gameDetailVM);
        }

        #endregion
    }
}

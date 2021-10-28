using AutoMapper;
using Business.Abstract;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Interfaces;

namespace _211_Studios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly ICategoryService _categoryService;

        public GameController(IGameService gameService, IMapper mapper, ILoggerManager loggerManager, ICategoryService categoryService)
        {
            _gameService = gameService;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _categoryService = categoryService;
        }

        [HttpGet("getGames/{skipCount}/{takeCount}")]
        public async Task<IActionResult> GetGamesAsync([FromRoute] int skipCount = 0, int takeCount = 6)
        {
            try
            {
                var games = await _gameService.GetGamesAsync(skipCount, takeCount);
                if (games is null)
                    return NotFound();

                var gamesDto = new List<GameDto>();
                foreach (var game in games)
                {
                    var gameDto = new GameDto
                    {
                        Id = game.Id,
                        Name = game.Name,
                        Description = game.Description,
                        Image = game.Image,
                        Category = _mapper.Map<CategoryDto>(game.GameCategories.FirstOrDefault(x => x.GameId == game.Id).Category)
                    };
                    gamesDto.Add(gameDto);
                }

                return Ok(gamesDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetGamesAsync)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getGamesByCategory/{categoryId}/{gameId}/{takeCount}")]
        public async Task<IActionResult> GetGamesAsync([FromRoute] int? categoryId, int gameId, int takeCount = 3)
        {
            try
            {
                if (categoryId is null)
                    return BadRequest();

                var games = await _gameService.GetGamesAsync(takeCount, categoryId.Value, gameId);
                if (games is null)
                    return NotFound();

                var gamesDto = new List<GameDto>();
                foreach (var game in games)
                {
                    var gameDto = new GameDto
                    {
                        Id = game.Id,
                        Name = game.Name,
                        Description = game.Description,
                        Image = game.Image,
                        Category = _mapper.Map<CategoryDto>(game.GameCategories.FirstOrDefault(x => x.GameId == game.Id).Category)
                    };
                    gamesDto.Add(gameDto);
                }

                return Ok(gamesDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetGamesAsync)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getGame/{id}")]
        public async Task<IActionResult> GetGame([FromRoute] int? id)
        {
            try
            {
                if (id is null)
                    return BadRequest();

                var game = await _gameService.GetGameWithIncludeAsync(id.Value);
                if (game is null)
                    return NotFound();

                var categories = new List<Category>();
                foreach (var item in game.GameCategories)
                {
                    categories.Add(item.Category);
                }

                var platforms = new List<Platform>();
                foreach (var platform in game.GameDetail.GameDetailPlatforms)
                {
                    platforms.Add(platform.Platform);
                }

                var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
                var platformsDto = _mapper.Map<List<PlatformDto>>(platforms);

                var gameDto = new GameDetailDto
                {
                    Id = game.Id,
                    Name = game.Name,
                    Description = game.Description,
                    Image = game.Image,
                    Video = game.GameDetail.Video,
                    CreationDate = game.GameDetail.CreationDate,
                    LastModificationDate = game.GameDetail.LastModificationDate,
                    Size = game.GameDetail.Size,
                    Categories = categoriesDto,
                    Platforms = platformsDto
                };


                return Ok(gameDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetGame)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getCategories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();
                if (categories is null)
                    return NotFound();

                var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

                return Ok(categoriesDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetCategories)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getGamesCount")]
        public async Task<IActionResult> GetGamesCount()
        {
            try
            {
                var games = await _gameService.GetGamesAsync();
                if (games is null)
                    return NotFound();

                return Ok(games.Count);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetGamesCount)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("getGamesByCategory/{skipCount}/{takeCount}")]
        public async Task<IActionResult> GetGamesByCategory([FromRoute] int skipCount, int takeCount, [FromQuery] int? categoryId)
        {
            List<Game> games = default;

            if (categoryId is null)
            {
                games = await _gameService.GetGamesAsync(skipCount, takeCount);
                if (games is null)
                    return NotFound();
            }
            else
            {
                games = await _gameService.GetGamesByCategoryAsync(skipCount, takeCount, categoryId.Value);
                if (games is null)
                    return NotFound();
            }


            var gamesDto = new List<GameDto>();
            foreach (var game in games)
            {
                var gameDto = new GameDto
                {
                    Id = game.Id,
                    Name = game.Name,
                    Description = game.Description,
                    Image = game.Image,
                    Category = _mapper.Map<CategoryDto>(game.GameCategories.FirstOrDefault(x => x.GameId == game.Id).Category)
                };
                gamesDto.Add(gameDto);
            }

            return Ok(gamesDto);
        }

        [HttpGet("getGamesCount/{categoryId}")]
        public async Task<IActionResult> GetGamesCount([FromRoute] int? categoryId)
        {
            try
            {
                if (categoryId is null)
                    return BadRequest();

                var games = await _gameService.GetGamesByCategoryAsync(categoryId.Value);
                if (games is null)
                    return NotFound();

                return Ok(games.Count);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetGamesCount)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

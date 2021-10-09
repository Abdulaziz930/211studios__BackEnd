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

        public GameController(IGameService gameService, IMapper mapper, ILoggerManager loggerManager)
        {
            _gameService = gameService;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        [HttpGet("getGames/{skipCount}/{takeCount}")]
        public async Task<IActionResult> GetGamesAsync([FromRoute] int skipCount = 0, int takeCount = 6)
        {
            try
            {
                var games = await _gameService.GetGamesAsync(skipCount, takeCount);
                if (games is null)
                    return NotFound();

                var gamesDto = _mapper.Map<List<GameDto>>(games);

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

                var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

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
                    Platform = game.GameDetail.Platform,
                    GameLink = game.GameDetail.GameLink,
                    Categories = categoriesDto
                };


                return Ok(gameDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetGame)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

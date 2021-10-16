using AutoMapper;
using Business.Abstract;
using Entities.DTOs;
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
    public class HomeController : ControllerBase
    {
        private readonly ISliderService _sliderService;
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public HomeController(ISliderService sliderService, IMapper mapper
            , ILoggerManager loggerManager, IGameService gameService)
        {
            _sliderService = sliderService;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _gameService = gameService;
        }

        [HttpGet("getSliders")]
        public async Task<IActionResult> GetSliders()
        {
            try
            {
                var sliders = await _sliderService.GetLastSlidersAsync();
                if (sliders is null)
                    return NotFound();

                var slidersDto = _mapper.Map<List<SliderDto>>(sliders);

                return Ok(slidersDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetSliders)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getGames/{takeCount}")]
        public async Task<IActionResult> GetGames([FromRoute] int takeCount = 6)
        {
            try
            {
                var games = await _gameService.GetGamesAsync(takeCount);
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
                _loggerManager.LogError($"Something went wrong in the {nameof(GetGames)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

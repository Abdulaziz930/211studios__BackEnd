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
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public HomeController(ISliderService sliderService, IMapper mapper, ILoggerManager loggerManager)
        {
            _sliderService = sliderService;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        [HttpGet("getSliders")]
        public async Task<IActionResult> GetSliders()
        {
            try
            {
                var sliders = await _sliderService.GetSlidersAsync();

                var slidersDto = _mapper.Map<List<SliderDto>>(sliders);

                return Ok(slidersDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetSliders)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

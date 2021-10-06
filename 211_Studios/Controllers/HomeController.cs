using AutoMapper;
using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _211_Studios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ISliderService _sliderService;
        private readonly IMapper _mapper;

        public HomeController(ISliderService sliderService, IMapper mapper)
        {
            _sliderService = sliderService;
            _mapper = mapper;
        }

        [HttpGet("getSliders")]
        public async Task<IActionResult> GetSliders()
        {
            var sliders = await _sliderService.GetSlidersAsync();

            var slidersDto = _mapper.Map<List<SliderDto>>(sliders);

            return Ok(slidersDto);
        }
    }
}

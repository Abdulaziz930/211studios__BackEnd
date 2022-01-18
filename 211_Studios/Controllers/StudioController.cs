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
    public class StudioController : ControllerBase
    {
        private readonly IStudioService _studioService;
        private readonly IBannerService _bannerService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public StudioController(IStudioService studioService, IMapper mapper, ILoggerManager loggerManager, IBannerService bannerService)
        {
            _studioService = studioService;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _bannerService = bannerService;
        }

        [HttpGet("getStudio")]
        public async Task<IActionResult> GetStudio()
        {
            try
            {
                var studio = await _studioService.GetStudioAsync();
                if (studio is null)
                    return NotFound();

                var banner = await _bannerService.GetBannerAsync("our studios");
                if (banner is null)
                    return NotFound();

                var studioDto = new StudioPageDto
                {
                    Id = studio.Id,
                    Title = studio.Title,
                    Description = studio.Description,
                    Image = studio.Image,
                    BannerTitle = banner.Title,
                    BannerDescription = banner.Description,
                    BannerImage = banner.Image
                };

                return Ok(studioDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetStudio)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

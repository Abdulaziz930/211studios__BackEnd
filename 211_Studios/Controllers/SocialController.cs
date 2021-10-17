using AutoMapper;
using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Interfaces;

namespace _211_Studios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialController : ControllerBase
    {
        private readonly ISocialService _socialService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public SocialController(ISocialService socialService, IMapper mapper, ILoggerManager loggerManager)
        {
            _socialService = socialService;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        [HttpGet("getSocialMedias")]
        public async Task<IActionResult> GetSocials()
        {
            try
            {
                var socials = await _socialService.GetSocialsAsync();
                if (socials is null)
                    return NotFound();

                var socialsDto = _mapper.Map<List<SocialDto>>(socials);

                return Ok(socialsDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetSocials)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

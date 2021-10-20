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
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public StudioController(IStudioService studioService, IMapper mapper, ILoggerManager loggerManager)
        {
            _studioService = studioService;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        [HttpGet("getStudio")]
        public async Task<IActionResult> GetStudio()
        {
            try
            {
                var studio = await _studioService.GetStudioAsync();
                if (studio is null)
                    return NotFound();

                var studioDto = _mapper.Map<StudioPageDto>(studio);

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

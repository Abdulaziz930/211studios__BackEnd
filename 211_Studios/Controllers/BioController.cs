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
    public class BioController : ControllerBase
    {
        private readonly IBioService _bioService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public BioController(IBioService bioService, IMapper mapper, ILoggerManager loggerManager)
        {
            _bioService = bioService;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        [HttpGet("getFooterBio")]
        public async Task<IActionResult> GetFooterBio()
        {
            try
            {
                var bios = await _bioService.GetBiosAsync();
                if (bios is null)
                    return NotFound();

                var bioDto = _mapper.Map<BioDto>(bios[0]);

                return Ok(bioDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetFooterBio)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getContactBio")]
        public async Task<IActionResult> GetContactBio()
        {
            try
            {
                var bios = await _bioService.GetBiosAsync();
                if (bios is null)
                    return NotFound();

                var bioDto = _mapper.Map<BioContactDto>(bios[0]);

                return Ok(bioDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetContactBio)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

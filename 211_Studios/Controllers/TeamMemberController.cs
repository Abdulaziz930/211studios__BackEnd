using AutoMapper;
using Business.Abstract;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Interfaces;

namespace _211_Studios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITeamMemberBannerService _teamMemberBannerService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public TeamMemberController(UserManager<AppUser> userManager, IMapper mapper
            , ILoggerManager loggerManager, ITeamMemberBannerService teamMemberBannerService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _teamMemberBannerService = teamMemberBannerService;
        }

        [HttpGet("getTeamMembers")]
        public async Task<IActionResult> GetTeamMembers()
        {
            try
            {
                var teamMembers = await _userManager.Users.ToListAsync();
                if (teamMembers is null)
                    return NotFound();

                var teamMembersDto = _mapper.Map<List<AppUserDto>>(teamMembers);

                return Ok(teamMembersDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetTeamMembers)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getTeamMemberDetail/{teamMemberId}")]
        public async Task<IActionResult> GetTeamMemberDetail([FromRoute] string teamMemberId)
        {
            try
            {
                if (teamMemberId is null)
                    return BadRequest();

                var teamMember = await _userManager.Users.Include(x => x.UserSocialMedias)
                    .FirstOrDefaultAsync(x => x.Id == teamMemberId);
                if (teamMember is null)
                    return NotFound();

                var userSocailMedias = teamMember.UserSocialMedias.Where(x => x.IsDeleted == false).ToList();

                var userSocialMediasDto = _mapper.Map<List<UserSocialMediaDto>>(userSocailMedias);

                var teamMemberDto = new AppUserDetailDto
                {
                    Id = teamMember.Id,
                    FullName = teamMember.FullName,
                    Position = teamMember.Position,
                    Description = teamMember.Description,
                    Image = teamMember.Image,
                    UserSocialMediasDto = userSocialMediasDto
                };

                return Ok(teamMemberDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetTeamMemberDetail)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getTeamMemberDetailBanner")]
        public async Task<IActionResult> GetTeamMemberDetailBanner()
        {
            try
            {
                var teamMemberBanners = await _teamMemberBannerService.GetTeamMembersAsync();
                if (teamMemberBanners is null)
                    return NotFound();

                var teamMemberBannerDto = _mapper.Map<TeamMemberBannerDto>(teamMemberBanners[0]);

                return Ok(teamMemberBannerDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetTeamMemberDetailBanner)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

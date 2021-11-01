using AutoMapper;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;
using Utils.Interfaces;

namespace _211_Studios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public ContactController(IMapper mapper, ILoggerManager loggerManager)
        {
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailDto sendEmailDto)
        {
            try
            {
                if (string.IsNullOrEmpty(sendEmailDto.Name))
                    return BadRequest("Name field is required");

                if (string.IsNullOrEmpty(sendEmailDto.Email))
                    return BadRequest("Email field is required");

                if (string.IsNullOrEmpty(sendEmailDto.Message))
                    return BadRequest("Message field is required");

                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(sendEmailDto.Email);
                if (match is null)
                    return BadRequest("Incorrect email field");

                string title = $"{sendEmailDto.Name}'s contact";
                string description = $"Email: {sendEmailDto.Email} <br /> Phone number: {sendEmailDto.PhoneNumber ?? ""} <br /> Message: {sendEmailDto.Message}";
                var homeLink = Constants.ClientPort;
                var filePath = Constants.ContactEmailFolderPath;
                var resultMessage = FileUtil.GetContactEmailView(filePath, title, description, homeLink);

                await EmailUtil.SendEmailAsync(Constants.EmailAdress, resultMessage, sendEmailDto.Subject);

                return Ok("Email succefuly sended");
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(SendEmail)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

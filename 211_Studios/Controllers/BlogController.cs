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
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public BlogController(IBlogService blogService, IMapper mapper, ILoggerManager loggerManager)
        {
            _blogService = blogService;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        [HttpGet("getBlogs/{skipCount}/{takeCount}")]
        public async Task<IActionResult> GetBlogs([FromRoute] int skipCount = 0, int takeCount = 6)
        {
            try
            {
                var blogs = await _blogService.GetBlogsAsync(skipCount, takeCount);
                if (blogs is null)
                    return NotFound();

                var blogsDto = _mapper.Map<List<BlogDto>>(blogs);

                return Ok(blogsDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetBlogs)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getBlogs/{userId}/{skipCount}/{takeCount}")]
        public async Task<IActionResult> GetBlogs([FromRoute] string userId, int skipCount = 0, int takeCount = 6)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest();

                var blogs = await _blogService.GetBlogsAsync(userId, skipCount, takeCount);
                if (blogs is null)
                    return NotFound();

                var blogsDto = _mapper.Map<List<BlogDto>>(blogs);

                return Ok(blogsDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetBlogs)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getBlog/{id}")]
        public async Task<IActionResult> GetBlog([FromRoute] int? id)
        {
            try
            {
                if (id is null)
                    return BadRequest();

                var blog = await _blogService.GetBlogWithIncludeAsync(id.Value);
                if (blog is null)
                    return NotFound();

                var blogDto = _mapper.Map<BlogDto>(blog);

                return Ok(blogDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetBlog)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string search)
        {
            try
            {
                if (string.IsNullOrEmpty(search.Trim()))
                    return BadRequest();

                var searchResult = await _blogService.SearchBlogAsync(search.Trim().ToLower());
                if (searchResult is null)
                    return NotFound();

                var searchResultDto = _mapper.Map<List<BlogDto>>(searchResult);

                return Ok(searchResultDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(Search)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getBlogsCount")]
        public async Task<IActionResult> GetBlogsCount()
        {
            try
            {
                var blogs = await _blogService.GetBlogsAsync();
                if (blogs is null)
                    return NotFound();

                var result = Decimal.Ceiling((decimal)blogs.Count / 4);

                return Ok(result);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetBlogsCount)} action {e}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getBlogs")]
        public async Task<IActionResult> GetBlogs([FromQuery] string userId, int? page = 1)
        {
            try
            {
                if (page is null || page == 0)
                    return NotFound();

                List<Blog> blogs = default;

                if (userId is null || userId.Trim() == "")
                {
                    var allBlogs = await _blogService.GetBlogsAsync();
                    if (allBlogs is null)
                        return NotFound();

                    var result = Decimal.Ceiling((decimal)allBlogs.Count / 4);
                    if (result < page || page <= 0)
                        return NotFound();

                    int skipCount = (page.Value - 1) * 4;

                    blogs = await _blogService.GetBlogsAsync(skipCount, 4);
                    if (blogs is null)
                        return NotFound();
                }
                else
                {
                    var allBlogs = await _blogService.GetBlogsAsync(userId);
                    if (allBlogs is null)
                        return NotFound();

                    var result = Decimal.Ceiling((decimal)allBlogs.Count / 4);
                    if (result < page || page <= 0)
                        return NotFound();

                    int skipCount = (page.Value - 1) * 4;

                    blogs = await _blogService.GetBlogsAsync(userId, skipCount, 4);
                    if (blogs is null)
                        return NotFound();
                }

                var blogsDto = _mapper.Map<List<BlogDto>>(blogs);

                return Ok(blogsDto);
            }
            catch (Exception e)
            {
                _loggerManager.LogError($"Something went wrong in the {nameof(GetBlogsCount)} action {e}");
                return StatusCode(500, "Internal server error");
            }
            
        }

    }
}

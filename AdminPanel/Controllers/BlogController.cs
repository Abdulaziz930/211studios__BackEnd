using AdminPanel.ViewModels;
using Business.Abstract;
using DataAccess.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;
using static Utils.CommonEnums;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = RoleConstants.AdminRole + "," + RoleConstants.ModeratorRole)]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<AppUser> _userManager;

        public BlogController(IBlogService blogService, UserManager<AppUser> userManager)
        {
            _blogService = blogService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allBlogs = await _blogService.GetBlogsAsync();
            ViewBag.PageCount = Decimal.Ceiling((decimal)allBlogs.Count / 5);
            ViewBag.Page = page;

            if (allBlogs.Count > 0 && (ViewBag.PageCount < page || page <= 0))
                return NotFound();

            int skipCount = (page - 1) * 5;

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            List<Blog> blogs;

            if (User.IsInRole(RoleConstants.AdminRole))
            {
                blogs = await _blogService.GetBlogsAsync(skipCount, 5);
            }
            else
            {
                blogs = await _blogService.GetBlogsAsync(user.Id, skipCount, 5);
            }

            if (blogs is null)
                return NotFound();

            var blogsVM = new List<BlogViewModel>();
            foreach (var blog in blogs)
            {
                var blogVM = new BlogViewModel
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Image = blog.Image,
                    CreationDate = blog.CreationDate,
                    LastModificationDate = blog.LastModificationDate
                };
                blogsVM.Add(blogVM);
            }

            return View(blogsVM);
        }

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (blog.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo field cannot be empty");
                return View();
            }

            if (!blog.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "This is not a picture");
                return View();
            }

            if (!blog.Photo.IsSizeAllowed(3000))
            {
                ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                return View();
            }

            var imageFileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, blog.Photo, FileType.Image);

            blog.Image = imageFileName;

            if (!ModelState.IsValid)
            {
                return View(blog);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            blog.CreationDate = DateTime.UtcNow;
            blog.LastModificationDate = DateTime.UtcNow;

            blog.AppUserId = user.Id;

            await _blogService.AddAsync(blog);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var blog = await _blogService.GetBlogWithIncludeAsync(id.Value);
            if (blog is null)
                return NotFound();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            if (!User.IsInRole(RoleConstants.AdminRole) && blog.AppUser.Id != user.Id)
                return BadRequest();

            var blogVM = new UpdateBlogViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Image = blog.Image
            };

            return View(blogVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, UpdateBlogViewModel blogVM)
        {
            if (id is null)
                return BadRequest();

            var dbBlog = await _blogService.GetBlogWithIncludeAsync(id.Value);
            if (dbBlog is null)
                return NotFound();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            if (!User.IsInRole(RoleConstants.AdminRole) && user.Id != dbBlog.AppUser.Id)
                return BadRequest();

            var imageFileName = dbBlog.Image;

            if (blogVM.Photo != null)
            {
                if (!blogVM.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "This is not a picture");
                    return View(dbBlog);
                }

                if (!blogVM.Photo.IsSizeAllowed(3000))
                {
                    ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                    return View(dbBlog);
                }

                imageFileName = await FileUtil.UpdateFileAsync(dbBlog.Image, Constants.ImageFolderPath, blogVM.Photo, FileType.Image);
            }

            if (!ModelState.IsValid)
            {
                return View(dbBlog);
            }

            dbBlog.Title = blogVM.Title;
            dbBlog.Content = blogVM.Content;
            dbBlog.Image = imageFileName;
            dbBlog.LastModificationDate = DateTime.UtcNow;

            await _blogService.UpdateAsync(dbBlog);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var blog = await _blogService.GetBlogWithIncludeAsync(id.Value);
            if (blog is null)
                return NotFound();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            if (!User.IsInRole(RoleConstants.AdminRole) && blog.AppUser.Id != user.Id)
                return BadRequest();

            var blogDetailVM = new BlogDetailViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Image = blog.Image,
                CreationDate = blog.CreationDate,
                LastModificationDate = blog.LastModificationDate
            };

            return View(blogDetailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteBlog(int? id)
        {
            if (id is null)
                return BadRequest();

            var blog = await _blogService.GetBlogWithIncludeAsync(id.Value);
            if (blog is null)
                return NotFound();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            if (!User.IsInRole(RoleConstants.AdminRole) && blog.AppUser.Id != user.Id)
                return BadRequest();

            blog.IsDeleted = true;

            await FileUtil.DeleteFileAsync(blog.Image, FileType.Image);

            await _blogService.UpdateAsync(blog);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var blog = await _blogService.GetBlogWithIncludeAsync(id.Value);
            if (blog is null)
                return NotFound();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user is null)
                return NotFound();

            if (!User.IsInRole(RoleConstants.AdminRole) && blog.AppUser.Id != user.Id)
                return BadRequest();

            var blogDetailVM = new BlogDetailViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Image = blog.Image,
                AuthorName = blog.AppUser.FullName,
                CreationDate = blog.CreationDate,
                LastModificationDate = blog.LastModificationDate
            };

            return View(blogDetailVM);
        }

        #endregion
    }
}

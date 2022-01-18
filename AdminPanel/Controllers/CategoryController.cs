using AdminPanel.ViewModels;
using Business.Abstract;
using DataAccess.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = RoleConstants.AdminRole + "," + RoleConstants.ModeratorRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allCategories = await _categoryService.GetCategoriesAsync();
            ViewBag.PageCount = Decimal.Ceiling((decimal)allCategories.Count / 5);
            ViewBag.Page = page;

            if (allCategories.Count > 0 && (ViewBag.PageCount < page || page <= 0))
                return NotFound();

            int skipCount = (page - 1) * 5;

            var categories = await _categoryService.GetCategoriesAsync(skipCount, 5);

            var categoriesVM = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                var categoryVM = new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                };
                categoriesVM.Add(categoryVM);
            }

            return View(categoriesVM);
        }

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            var isExist = await _categoryService.CheckCategoryAsync(x => x.IsDeleted == false && x.Name.ToLower() == category.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name", "There is a category with this name");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            await _categoryService.AddAsync(category);

            return RedirectToAction("Index");
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var category = await _categoryService.GetCategoryAsync(id.Value);
            if (category is null)
                return NotFound();

            var categoryVM = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryViewModel categoryVM)
        {
            if (id is null)
                return BadRequest();

            if (categoryVM.Id != id)
                return BadRequest();

            var category = await _categoryService.GetCategoryAsync(id.Value);
            if (category is null)
                return NotFound();

            var isExist = await _categoryService
                .CheckCategoryAsync(x => x.IsDeleted == false && x.Name.ToLower() == categoryVM.Name.ToLower() && x.Id != category.Id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "There is a category with this name");
                return View(categoryVM);
            }

            if (!ModelState.IsValid)
            {
                return View(categoryVM);
            }

            category.Name = categoryVM.Name;

            await _categoryService.UpdateAsync(category);

            return RedirectToAction("Index");
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var category = await _categoryService.GetCategoryAsync(id.Value);
            if (category is null)
                return NotFound();

            var categoryVM = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteSlider(int? id)
        {
            if (id is null)
                return BadRequest();

            var category = await _categoryService.GetCategoryAsync(id.Value);
            if (category is null)
                return NotFound();

            category.IsDeleted = true;

            await _categoryService.UpdateAsync(category);

            return RedirectToAction("Index");
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var category = await _categoryService.GetCategoryAsync(id.Value);
            if (category is null)
                return NotFound();

            var categoryVM = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(categoryVM);
        }

        #endregion
    }
}

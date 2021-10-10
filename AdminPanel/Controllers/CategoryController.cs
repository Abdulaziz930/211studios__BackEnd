using AdminPanel.ViewModels;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Controllers
{
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
    }
}

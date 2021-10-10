using AdminPanel.ViewModels;
using Business.Abstract;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace AdminPanel.Controllers
{
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allSliders = await _sliderService.GetSlidersAsync();
            ViewBag.PageCount = Decimal.Ceiling((decimal)allSliders.Count / 5);
            ViewBag.Page = page;

            if (allSliders.Count > 0 && (ViewBag.PageCount < page || page <= 0))
                return NotFound();

            int skipCount = (page - 1) * 5;

            var sliders = await _sliderService.GetSlidersAsync(skipCount, 5);

            var slidersVM = new List<SliderViewModel>();
            foreach (var sliderItem in sliders)
            {
                var sliderVM = new SliderViewModel
                {
                    Id = sliderItem.Id,
                    Title = sliderItem.Title,
                    Image = sliderItem.Image
                };
                slidersVM.Add(sliderVM);
            }

            return View(slidersVM);
        }

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Photo field cannot be empty");
                return View();
            }

            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "This is not a picture");
                return View();
            }

            if (!slider.Photo.IsSizeAllowed(3000))
            {
                ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                return View();
            }

            var fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, slider.Photo);

            slider.Image = fileName;

            if (!ModelState.IsValid)
            {
                return View(slider);
            }

            await _sliderService.AddAsync(slider);

            return RedirectToAction("Index");
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var slider = await _sliderService.GetSliderAsync(id.Value);
            if (slider is null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Slider slider)
        {
            if (id is null)
                return BadRequest();

            if (id != slider.Id)
                return BadRequest();

            var dbSlider = await _sliderService.GetSliderAsync(id.Value);
            if (dbSlider is null)
                return NotFound();

            var fileName = dbSlider.Image;

            if (slider.Photo != null)
            {
                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "This is not a picture");
                    return View();
                }

                if (!slider.Photo.IsSizeAllowed(3000))
                {
                    ModelState.AddModelError("Photo", "The size of the image you uploaded is 3 MB higher.");
                    return View();
                }

                var path = Path.Combine(Constants.ImageFolderPath, dbSlider.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                fileName = await FileUtil.GenerateFileAsync(Constants.ImageFolderPath, slider.Photo);
            }

            if (!ModelState.IsValid)
            {
                return View(dbSlider);
            }

            dbSlider.Title = slider.Title;
            dbSlider.Image = fileName;

            await _sliderService.UpdateAsync(dbSlider);

            return RedirectToAction("Index");
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var slider = await _sliderService.GetSliderAsync(id.Value);
            if (slider is null)
                return NotFound();

            var sliderVM = new SliderViewModel
            {
                Id = slider.Id,
                Title = slider.Title,
                Image = slider.Image
            };

            return View(sliderVM);
        }

        #endregion
    }
}

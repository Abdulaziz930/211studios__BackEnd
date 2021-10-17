using AdminPanel.ViewModels;
using Business.Abstract;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdminPanel.Controllers
{
    public class BioController : Controller
    {
        private readonly IBioService _bioService;

        public BioController(IBioService bioService)
        {
            _bioService = bioService;
        }

        public async Task<IActionResult> Index()
        {
            var bios = await _bioService.GetBiosAsync();
            if (bios is null)
                return NotFound();

            var bioVM = new BioViewModel
            {
                Id = bios.FirstOrDefault().Id,
                Email = bios.FirstOrDefault().Email,
                PhoneNumber = bios.FirstOrDefault().PhoneNumber,
            };

            return View(bioVM);
        }

        #region Create

        public async Task<IActionResult> Create()
        {
            var bios = await _bioService.GetBiosAsync();
            if (bios.Count > 0)
                return BadRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bio bio)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var phoneRegex = new Regex(@"^\+994[0-9]{2}[0-9]{3}[0-9]{2}[0-9]{2}$");
            if (!phoneRegex.IsMatch(bio.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Please enter the phone number in the correct format");
                return View();
            }

            await _bioService.AddAsync(bio);

            return RedirectToAction("Index");
        }

        #endregion

        #region Update

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            var bio = await _bioService.GetBioAsync(id.Value);
            if (bio is null)
                return NotFound();

            return View(bio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Bio bio)
        {
            if (id is null)
                return BadRequest();

            var dbBio = await _bioService.GetBioAsync(id.Value);
            if (dbBio is null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View(bio);
            }

            var phoneRegex = new Regex(@"^\+994[0-9]{2}[0-9]{3}[0-9]{2}[0-9]{2}$");
            if (!phoneRegex.IsMatch(bio.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Please enter the phone number in the correct format");
                return View(bio);
            }

            dbBio.Email = bio.Email;
            dbBio.FooterDescription = bio.FooterDescription;
            dbBio.PhoneNumber = bio.PhoneNumber;
            dbBio.Address = bio.Address;

            await _bioService.UpdateAsync(dbBio);

            return RedirectToAction("Index");
        }

        #endregion

        #region Detail

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null)
                return BadRequest();

            var bio = await _bioService.GetBioAsync(id.Value);
            if (bio is null)
                return NotFound();

            return View(bio);
        }

        #endregion
    }
}

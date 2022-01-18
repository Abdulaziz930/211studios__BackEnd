using AdminPanel.ViewModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public NavigationViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var userVM = new NaviUserViewModel
            {
                UserName = user.UserName,
                Image = user.Image
            };

            return View(userVM);
        }
    }
}

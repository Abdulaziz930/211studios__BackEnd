using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concret
{
    public class EFBannerDal : EFRepositoryBase<Banner, AppDbContext>, IBannerDal
    {
        public EFBannerDal(AppDbContext context) : base(context)
        {
        }

        public async Task<Banner> GetBannerAsync(string pageName)
        {
            return await Context.Banners.Include(x => x.Menu)
                .FirstOrDefaultAsync(x => x.IsDeleted == false && x.Menu.PageName.ToLower() == pageName.ToLower());
        }
    }
}

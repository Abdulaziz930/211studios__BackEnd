using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concret
{
    public class EFBannerDal : EFRepositoryBase<Banner, AppDbContext>, IBannerDal
    {
        public EFBannerDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckBannerAsync(Expression<Func<Banner, bool>> filter)
        {
            return await Context.Banners.AnyAsync(filter);
        }

        public async Task<List<Banner>> GetAllBannersAsync()
        {
            return await Context.Banners.Include(x => x.Menu).Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.LastModificationDate).ToListAsync();
        }

        public async Task<Banner> GetBannerAsync(string pageName)
        {
            return await Context.Banners.Include(x => x.Menu)
                .FirstOrDefaultAsync(x => x.IsDeleted == false && x.Menu.PageName.ToLower() == pageName.ToLower());
        }

        public async Task<Banner> GetBannerWithIncludeAsync(int id)
        {
            return await Context.Banners.Include(x => x.Menu).FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
        }
    }
}

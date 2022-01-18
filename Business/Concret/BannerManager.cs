using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concret
{
    public class BannerManager : IBannerService
    {
        private readonly IBannerDal _bannerDal;

        public BannerManager(IBannerDal bannerDal)
        {
            _bannerDal = bannerDal;
        }

        public async Task<bool> AddAsync(Banner banner)
        {
            return await _bannerDal.AddAsync(banner);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _bannerDal.DeleteAsync(new Banner { Id = id });
        }

        public async Task<Banner> GetBannerAsync(string pageName)
        {
            return await _bannerDal.GetBannerAsync(pageName);
        }

        public async Task<Banner> GetBannerAsync(int id)
        {
            return await _bannerDal.GetAsync(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task<List<Banner>> GetBannersAsync()
        {
            return await _bannerDal.GetAllAsync(x => x.IsDeleted == false);
        }

        public async Task<bool> UpdateAsync(Banner banner)
        {
            return await _bannerDal.UpdateAsync(banner);
        }
    }
}

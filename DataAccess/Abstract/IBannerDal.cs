using Core.Repository;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IBannerDal : IRepository<Banner>
    {
        Task<Banner> GetBannerAsync(string pageName);

        Task<Banner> GetBannerWithIncludeAsync(int id);

        Task<List<Banner>> GetAllBannersAsync();

        Task<bool> CheckBannerAsync(Expression<Func<Banner, bool>> filter);
    }
}

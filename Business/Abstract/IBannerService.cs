using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBannerService
    {
        Task<Banner> GetBannerAsync(string pageName);

        Task<Banner> GetBannerAsync(int id);

        Task<List<Banner>> GetBannersAsync();

        Task<bool> AddAsync(Banner banner);

        Task<bool> UpdateAsync(Banner banner);

        Task<bool> DeleteAsync(int id);
    }
}

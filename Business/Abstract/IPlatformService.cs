using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IPlatformService
    {
        Task<Platform> GetPlatformAsync(int id);

        Task<List<Platform>> GetPlatformsAsync();

        Task<List<Platform>> GetPlatformsAsync(int skipCount, int takeCount);

        Task<bool> CheckPlatformAsync(Expression<Func<Platform, bool>> filter);

        Task<bool> AddAsync(Platform platform);

        Task<bool> UpdateAsync(Platform platform);

        Task<bool> DeleteAsync(int id);
    }
}

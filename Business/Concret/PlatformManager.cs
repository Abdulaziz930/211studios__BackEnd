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
    public class PlatformManager : IPlatformService
    {
        private readonly IPlatformDal _platformDal;
        public PlatformManager(IPlatformDal platformDal)
        {
            _platformDal = platformDal;
        }

        public async Task<bool> AddAsync(Platform platform)
        {
            await _platformDal.AddAsync(platform);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _platformDal.DeleteAsync(new Platform { Id = id });

            return true;
        }

        public async Task<Platform> GetPlatformAsync(int id)
        {
            return await _platformDal.GetAsync(x => x.IsDeleted == false && x.Id == id);
        }

        public async Task<List<Platform>> GetPlatformsAsync()
        {
            return await _platformDal.GetAllAsync(x => x.IsDeleted == false);
        }

        public async Task<List<Platform>> GetPlatformsAsync(int skipCount, int takeCount)
        {
            return await _platformDal.GetPlatformsBySkipAndTakeCountAsync(skipCount, takeCount);
        }

        public async Task<bool> UpdateAsync(Platform platform)
        {
            await _platformDal.UpdateAsync(platform);

            return true;
        }
    }
}

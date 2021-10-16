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
    public class StudioManager : IStudioService
    {
        private readonly IStudioDal _studioDal;

        public StudioManager(IStudioDal studioDal)
        {
            _studioDal = studioDal;
        }

        public async Task<bool> AddAsync(Studio studio)
        {
            return await _studioDal.AddAsync(studio);
        }

        public async Task<bool> AddRangeAsync(Studio studio, StudioDetail studioDetail)
        {
            return await _studioDal.AddRangeAsync(studio, studioDetail);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _studioDal.DeleteAsync(new Studio { Id = id });
        }

        public async Task<Studio> GetStudioAsync(int id)
        {
            return await _studioDal.GetAsync(x => x.Id == id);
        }

        public async Task<List<Studio>> GetStudiosAsync()
        {
            return await _studioDal.GetAllAsync();
        }

        public async Task<Studio> GetStudioWithIncludeAsync(int id)
        {
            return await _studioDal.GetStudiWithStudioDetailAsync(id);
        }

        public async Task<bool> UpdateAsync(Studio studio)
        {
            return await _studioDal.UpdateAsync(studio);
        }
    }
}

using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concret
{
    public class SocialManager : ISocialService
    {
        private readonly ISocialDal _socialDal;

        public SocialManager(ISocialDal socialDal)
        {
            _socialDal = socialDal;
        }

        public async Task<bool> AddAsync(Social social)
        {
            return await _socialDal.AddAsync(social);
        }

        public async Task<bool> CheckSocialAsync(Expression<Func<Social, bool>> filter)
        {
            return await _socialDal.CheckSocialAsync(filter);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _socialDal.DeleteAsync(new Social { Id = id });
        }

        public async Task<Social> GetSocialAsync(int id)
        {
            return await _socialDal.GetAsync(x => x.IsDeleted == false && x.Id == id);
        }

        public async Task<List<Social>> GetSocialsAsync()
        {
            return await _socialDal.GetAllAsync(x => x.IsDeleted == false);
        }

        public async Task<List<Social>> GetSocialsAsync(int skipCount, int takeCount)
        {
            return await _socialDal.GetSocialsByTakeAndSkipCountAsync(skipCount, takeCount);
        }

        public async Task<bool> UpdateAsync(Social social)
        {
            return await _socialDal.UpdateAsync(social);
        }
    }
}

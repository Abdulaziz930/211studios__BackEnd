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
    public class UserSocialManager : IUserSocialService
    {
        private readonly IUserSocialDal _userSocial;

        public UserSocialManager(IUserSocialDal userSocial)
        {
            _userSocial = userSocial;
        }

        public async Task<bool> AddUserSocialAsync(UserSocialMedia userSocial)
        {
            return await _userSocial.AddAsync(userSocial);
        }

        public async Task<bool> DeleteUserSocialAsync(int id)
        {
            return await _userSocial.DeleteAsync(new UserSocialMedia { Id = id });
        }

        public async Task<UserSocialMedia> GetUserSocialAsync(int id)
        {
            return await _userSocial.GetAsync(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task<List<UserSocialMedia>> GetUserSocialsAsync()
        {
            return await _userSocial.GetAllAsync(x => x.IsDeleted == false);
        }

        public async Task<List<UserSocialMedia>> GetUserSocialsAsync(string userId, int skipCount, int takeCount)
        {
            return await _userSocial.GetUserSocialMediasBySkipAndTakeCountAsync(userId, skipCount, takeCount);
        }

        public async Task<bool> UpdateUserSocialAsync(UserSocialMedia userSocial)
        {
            return await _userSocial.UpdateAsync(userSocial);
        }
    }
}

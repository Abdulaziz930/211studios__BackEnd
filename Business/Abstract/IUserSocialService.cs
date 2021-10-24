using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserSocialService
    {
        Task<UserSocialMedia> GetUserSocialAsync(int id);
        
        Task<List<UserSocialMedia>> GetUserSocialsAsync();
        
        Task<List<UserSocialMedia>> GetUserSocialsAsync(string userId, int skipCount, int takeCount);

        Task<bool> AddUserSocialAsync(UserSocialMedia userSocial);

        Task<bool> UpdateUserSocialAsync(UserSocialMedia userSocial);

        Task<bool> DeleteUserSocialAsync(int id);
    }
}

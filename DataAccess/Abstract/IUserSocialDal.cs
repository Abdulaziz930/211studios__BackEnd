using Core.Repository;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IUserSocialDal : IRepository<UserSocialMedia>
    {
        Task<List<UserSocialMedia>> GetUserSocialMediasBySkipAndTakeCountAsync(string userId, int skipCount, int takeCount);
    }
}

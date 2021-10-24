using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concret
{
    public class EFUserSocialDal : EFRepositoryBase<UserSocialMedia, AppDbContext>, IUserSocialDal
    {
        public EFUserSocialDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<UserSocialMedia>> GetUserSocialMediasBySkipAndTakeCountAsync(string userId, int skipCount, int takeCount)
        {
            return await Context.UserSocialMedias.Include(x => x.AppUser).Where(x => x.IsDeleted == false && x.AppUserId == userId).ToListAsync();
        }
    }
}

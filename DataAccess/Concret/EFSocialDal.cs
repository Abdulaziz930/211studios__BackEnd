using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concret
{
    public class EFSocialDal : EFRepositoryBase<Social, AppDbContext>, ISocialDal
    {
        public EFSocialDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckSocialAsync(Expression<Func<Social, bool>> filter)
        {
            return await Context.Socials.AnyAsync(filter);
        }

        public async Task<List<Social>> GetSocialsByTakeAndSkipCountAsync(int skipCount, int takeCount)
        {
            return await Context.Socials.Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.Id).Skip(skipCount).Take(takeCount).ToListAsync();
        }
    }
}

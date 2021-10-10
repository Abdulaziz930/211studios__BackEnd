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
    public class EFPlatformDal : EFRepositoryBase<Platform, AppDbContext>, IPlatformDal
    {
        public EFPlatformDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Platform>> GetPlatformsBySkipAndTakeCountAsync(int skipCount, int takeCount)
        {
            return await Context.Platforms.Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.Id).Skip(skipCount).Take(takeCount).ToListAsync();
        }
    }
}

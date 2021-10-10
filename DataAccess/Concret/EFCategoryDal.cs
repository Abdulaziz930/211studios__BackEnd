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
    public class EFCategoryDal : EFRepositoryBase<Category, AppDbContext>, ICategoryDal
    {
        public EFCategoryDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Category>> GetCategoriesBySkipAndTakeCountAsync(int skipCount, int takeCount)
        {
            return await Context.Categories.Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.Id).Take(takeCount).Skip(skipCount).ToListAsync();
        }
    }
}

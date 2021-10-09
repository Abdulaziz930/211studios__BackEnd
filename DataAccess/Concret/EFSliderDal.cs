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
    public class EFSliderDal : EFRepositoryBase<Slider, AppDbContext>, ISliderDal
    {
        public EFSliderDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Slider>> GetSlidersBySkipAndTakeCount(int skipCount, int takeCount)
        {
            return await Context.Sliders.Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.Id).Skip(skipCount).Take(takeCount).ToListAsync();
        }
    }
}

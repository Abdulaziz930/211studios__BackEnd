using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
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
    }
}

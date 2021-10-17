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
    public class EFBioDal : EFRepositoryBase<Bio, AppDbContext>, IBioDal
    {
        public EFBioDal(AppDbContext context) : base(context)
        {
        }
    }
}

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
    public class EFTeamMemberBannerDal : EFRepositoryBase<TeamMemberBanner, AppDbContext>, ITeamMemberBannerDal
    {
        public EFTeamMemberBannerDal(AppDbContext context) : base(context)
        {
        }
    }
}

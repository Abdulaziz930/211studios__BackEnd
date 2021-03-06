using Core.Repository;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IPlatformDal : IRepository<Platform>
    {
        Task<List<Platform>> GetPlatformsBySkipAndTakeCountAsync(int skipCount, int takeCount);

        Task<bool> CheckPlatformAsync(Expression<Func<Platform, bool>> filter);
    }
}

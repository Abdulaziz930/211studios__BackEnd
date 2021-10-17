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
    public interface ISocialDal : IRepository<Social>
    {
        Task<List<Social>> GetSocialsByTakeAndSkipCountAsync(int skipCount, int takeCount);

        Task<bool> CheckSocialAsync(Expression<Func<Social, bool>> filter);
    }
}

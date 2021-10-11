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
    public interface ICategoryDal : IRepository<Category>
    {
        Task<List<Category>> GetCategoriesBySkipAndTakeCountAsync(int skipCount, int takeCount);

        Task<bool> CheckCategoryAsync(Expression<Func<Category, bool>> filter);
    }
}

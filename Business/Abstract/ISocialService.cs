using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISocialService
    {
        Task<Social> GetSocialAsync(int id);

        Task<List<Social>> GetSocialsAsync();

        Task<List<Social>> GetSocialsAsync(int skipCount, int takeCount);

        Task<bool> AddAsync(Social social);

        Task<bool> UpdateAsync(Social social);

        Task<bool> DeleteAsync(int id);

        Task<bool> CheckSocialAsync(Expression<Func<Social, bool>> filter);
    }
}

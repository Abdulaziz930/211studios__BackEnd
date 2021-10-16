using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IStudioService
    {
        Task<Studio> GetStudioAsync(int id);

        Task<Studio> GetStudioWithIncludeAsync(int id);

        Task<List<Studio>> GetStudiosAsync();

        Task<bool> AddAsync(Studio studio);

        Task<bool> AddRangeAsync(Studio studio, StudioDetail studioDetail);

        Task<bool> UpdateAsync(Studio studio);

        Task<bool> DeleteAsync(int id);
    }
}

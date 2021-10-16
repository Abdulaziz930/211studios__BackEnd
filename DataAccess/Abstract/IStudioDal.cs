using Core.Repository;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IStudioDal : IRepository<Studio>
    {
        Task<Studio> GetStudiWithStudioDetailAsync(int id);

        Task<bool> AddRangeAsync(Studio studio, StudioDetail studioDetail);
    }
}

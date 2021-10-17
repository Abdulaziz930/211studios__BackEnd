using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBioService
    {
        Task<Bio> GetBioAsync(int id);

        Task<List<Bio>> GetBiosAsync();

        Task<bool> AddAsync(Bio bio);

        Task<bool> UpdateAsync(Bio bio);

        Task<bool> DeleteAsync(int id);
    }
}

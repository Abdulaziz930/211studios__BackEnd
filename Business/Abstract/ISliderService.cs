using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISliderService
    {
        Task<Slider> GetSliderAsync(int id);

        Task<List<Slider>> GetSlidersAsync();

        Task<bool> AddAsync(Slider slider);

        Task<bool> UpdateAsync(Slider slider);

        Task<bool> DeleteAsync(int id);
    }
}

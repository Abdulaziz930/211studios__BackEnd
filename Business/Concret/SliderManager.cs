using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concret
{
    public class SliderManager : ISliderService
    {
        private readonly ISliderDal _sliderDal;

        public SliderManager(ISliderDal sliderDal)
        {
            _sliderDal = sliderDal;
        }

        public async Task<bool> AddAsync(Slider slider)
        {
            await _sliderDal.AddAsync(slider);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _sliderDal.DeleteAsync(new Slider { Id = id });

            return true;
        }

        public async Task<Slider> GetSliderAsync(int id)
        {
            return await _sliderDal.GetAsync(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task<List<Slider>> GetSlidersAsync()
        {
            return await _sliderDal.GetAllAsync(x => x.IsDeleted == false);
        }

        public async Task<List<Slider>> GetSlidersAsync(int skipCount, int takeCount)
        {
            return await _sliderDal.GetSlidersBySkipAndTakeCount(skipCount, takeCount);
        }

        public async Task<bool> UpdateAsync(Slider slider)
        {
            await _sliderDal.UpdateAsync(slider);

            return true;
        }
    }
}

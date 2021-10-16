using Core.Repository;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ISliderDal : IRepository<Slider>
    {
        Task<List<Slider>> GetSlidersBySkipAndTakeCount(int skipCount, int takeCount);

        Task<List<Slider>> GetSlidersByLastModeficationDateAsync();
    }
}

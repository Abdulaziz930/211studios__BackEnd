using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryAsync(int id);

        Task<List<Category>> GetCategoriesAsync();

        Task<List<Category>> GetCategoriesAsync(int skipCount, int takeCount);

        Task<bool> AddAsync(Category category);

        Task<bool> UpdateAsync(Category category);

        Task<bool> DeleteAsync(int id);
    }
}

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
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public async Task<bool> AddAsync(Category category)
        {
            await _categoryDal.AddAsync(category);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _categoryDal.DeleteAsync(new Category { Id = id });

            return true;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryDal.GetAllAsync(x => x.IsDeleted == false);
        }

        public async Task<List<Category>> GetCategoriesAsync(int skipCount, int takeCount)
        {
            return await _categoryDal.GetCategoriesBySkipAndTakeCountAsync(skipCount, takeCount);
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _categoryDal.GetAsync(x => x.IsDeleted == false && x.Id == id);
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            await _categoryDal.UpdateAsync(category);

            return true;
        }
    }
}

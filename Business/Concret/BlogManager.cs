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
    public class BlogManager : IBlogService
    {
        private readonly IBlogDal _blogDal;

        public BlogManager(IBlogDal blogDal)
        {
            _blogDal = blogDal;
        }

        public async Task<bool> AddAsync(Blog blog)
        {
            return await _blogDal.AddAsync(blog);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _blogDal.DeleteAsync(new Blog { Id = id });
        }

        public async Task<Blog> GetBlogAsync(int id)
        {
            return await _blogDal.GetAsync(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task<List<Blog>> GetBlogsAsync()
        {
            return await _blogDal.GetAllAsync(x => x.IsDeleted == false);
        }

        public async Task<List<Blog>> GetBlogsAsync(string userId, int skipCount, int takeCount)
        {
            return await _blogDal.GetBlogsBySkipAndTakeCountAsync(userId, skipCount, takeCount);
        }

        public async Task<List<Blog>> GetBlogsAsync(string userId)
        {
            return await _blogDal.GetUserBlogsAsync(userId);
        }

        public async Task<List<Blog>> GetBlogsAsync(int skipCount, int takeCount)
        {
            return await _blogDal.GetBlogsBySkipAndTakeCountAsync(skipCount, takeCount);
        }

        public async Task<Blog> GetBlogWithIncludeAsync(int id)
        {
            return await _blogDal.GetBlogWithIncludeAsync(id);
        }

        public async Task<List<Blog>> SearchBlogAsync(string search)
        {
            return await _blogDal.SearchBlogAsync(search);
        }

        public async Task<bool> UpdateAsync(Blog blog)
        {
            return await _blogDal.UpdateAsync(blog);
        }
    }
}

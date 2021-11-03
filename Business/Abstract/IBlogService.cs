using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBlogService
    {
        Task<Blog> GetBlogAsync(int id);

        Task<Blog> GetBlogWithIncludeAsync(int id);

        Task<List<Blog>> GetBlogsAsync();

        Task<List<Blog>> GetBlogsAsync(string userId, int skipCount, int takeCount);

        Task<List<Blog>> GetBlogsAsync(string userId);

        Task<bool> AddAsync(Blog blog);

        Task<bool> UpdateAsync(Blog blog);

        Task<bool> DeleteAsync(int id);
    }
}

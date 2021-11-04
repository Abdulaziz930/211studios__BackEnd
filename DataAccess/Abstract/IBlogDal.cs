using Core.Repository;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IBlogDal : IRepository<Blog>
    {
        Task<List<Blog>> GetBlogsBySkipAndTakeCountAsync(string userId, int skipCount, int takeCount);

        Task<List<Blog>> GetBlogsBySkipAndTakeCountAsync(int skipCount, int takeCount);

        Task<Blog> GetBlogWithIncludeAsync(int id);

        Task<List<Blog>> GetUserBlogsAsync(string userId);

        Task<List<Blog>> SearchBlogAsync(string search);
    }
}

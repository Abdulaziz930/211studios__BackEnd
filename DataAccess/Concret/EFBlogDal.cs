using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concret
{
    public class EFBlogDal : EFRepositoryBase<Blog, AppDbContext>, IBlogDal
    {
        public EFBlogDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Blog>> GetBlogsBySkipAndTakeCountAsync(string userId, int skipCount, int takeCount)
        {
            return await Context.Blogs.Include(x => x.AppUser)
                .Where(x => x.AppUser.Id == userId && x.IsDeleted == false && x.AppUser.IsActive).OrderByDescending(x => x.LastModificationDate)
                .Skip(skipCount).Take(takeCount).ToListAsync();
        }

        public async Task<List<Blog>> GetBlogsBySkipAndTakeCountAsync(int skipCount, int takeCount)
        {
            return await Context.Blogs.Include(x => x.AppUser)
                .Where(x => x.IsDeleted == false && x.AppUser.IsActive).OrderByDescending(x => x.LastModificationDate)
                .Skip(skipCount).Take(takeCount).ToListAsync();
        }

        public async Task<Blog> GetBlogWithIncludeAsync(int id)
        {
            return await Context.Blogs.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false && x.AppUser.IsActive);
        }

        public async Task<List<Blog>> GetUserBlogsAsync(string userId)
        {
            return await Context.Blogs.Include(x => x.AppUser)
                .Where(x => x.IsDeleted == false && x.AppUser.Id == userId && x.AppUser.IsActive).ToListAsync();
        }

        public async Task<List<Blog>> SearchBlogAsync(string search)
        {
            return await Context.Blogs.Include(x => x.AppUser)
                .Where(x => x.Title.Contains(search) && x.IsDeleted == false && x.AppUser.IsActive == true)
                .OrderByDescending(x => x.LastModificationDate).ToListAsync();
        }
    }
}

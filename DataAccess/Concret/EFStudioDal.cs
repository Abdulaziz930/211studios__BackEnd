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
    public class EFStudioDal : EFRepositoryBase<Studio, AppDbContext>, IStudioDal
    {
        public EFStudioDal(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AddRangeAsync(Studio studio, StudioDetail studioDetail)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                await Context.AddRangeAsync(studio, studioDetail);
                await Context.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Studio> GetStudiWithStudioDetailAsync(int id)
        {
            return await Context.Studios.Include(x => x.StudioDetail).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

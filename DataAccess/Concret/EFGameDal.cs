using Core.Repository.EFRepository;
using DataAccess.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concret
{
    public class EFGameDal : EFRepositoryBase<Game, AppDbContext>, IGameDal
    {
        public EFGameDal(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Game>> GetGamesBySkipAndTakeCountAsync(int skipCount, int takeCount)
        {
            return await Context.Games.Include(x => x.GameDetail).Include(x => x.GameCategories).ThenInclude(x => x.Category)
                .Where(x => x.IsDeleted == false && x.GameCategories.Any(x => x.Category.IsDeleted == false))
                .OrderByDescending(x => x.GameDetail.LastModificationDate)
                .Skip(skipCount).Take(takeCount).ToListAsync();
        }

        public async Task<List<Game>> GetGamesByTakeCountAsync(int takeCount)
        {
            return await Context.Games.Include(x => x.GameCategories).ThenInclude(x => x.Category)
                .Where(x => x.IsDeleted == false && x.GameCategories.Any(x => x.Category.IsDeleted == false))
                .OrderByDescending(x => x.GameDetail.LastModificationDate)
                .Take(takeCount).ToListAsync();
        }

        public async Task<Game> GetGameWithIncludeAsync(int id)
        {
            return await Context.Games.Include(x => x.GameDetail)
                .ThenInclude(x => x.GameDetailPlatforms).ThenInclude(x => x.Platform)
                .Include(x => x.GameCategories).ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == id 
                                    && x.GameCategories.Any(x => x.Category.IsDeleted == false && x.GameId == id));
        }

        public async Task<bool> AddRangeAsync(Game game, GameDetail gameDetail)
        {
            await using var dbContextTransaction = await Context.Database.BeginTransactionAsync();
            try
            {
                await Context.AddRangeAsync(game, gameDetail);
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

        public async Task<bool> CheckGameAsync(Expression<Func<Game, bool>> filter)
        {
            return await Context.Games.AnyAsync(filter);
        }

        public async Task<List<Game>> GetGamesByCategoryAsync(int takeCount, int categoryId, int gameId)
        {
            return await Context.Games.Include(x => x.GameCategories).ThenInclude(x => x.Category)
                .Where(x => x.IsDeleted == false && x.GameCategories.Any(x => x.Category.IsDeleted == false && x.CategoryId == categoryId) && x.Id != gameId)
                .OrderByDescending(x => x.GameDetail.LastModificationDate)
                .Take(takeCount).ToListAsync();
        }

        public async Task<List<Game>> GetGamesByFilteredSkipAndTakeCountAsync(int skipCount, int takeCount, int categoryId)
        {
            return await Context.Games.Include(x => x.GameCategories).ThenInclude(x => x.Category)
                .Where(x => x.IsDeleted == false && x.GameCategories.Any(x => x.Category.IsDeleted == false && x.CategoryId == categoryId))
                .OrderByDescending(x => x.GameDetail.LastModificationDate).Skip(skipCount).Take(takeCount).ToListAsync();
        }

        public async Task<List<Game>> GetGamesByCategoryAsync(int categoryId)
        {
            return await Context.Games.Include(x => x.GameCategories).ThenInclude(x => x.Category)
                .Where(x => x.IsDeleted == false && x.GameCategories.Any(x => x.Category.IsDeleted == false && x.CategoryId == categoryId))
                .ToListAsync();
        }
    }
}

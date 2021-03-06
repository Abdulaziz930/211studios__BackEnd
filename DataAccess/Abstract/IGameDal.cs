using Core.Repository;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IGameDal : IRepository<Game>
    {
        Task<List<Game>> GetGamesByTakeCountAsync(int takeCount);

        Task<List<Game>> GetGamesByCategoryAsync(int takeCount, int categoryId, int gameId);

        Task<List<Game>> GetGamesByCategoryAsync(int categoryId);

        Task<List<Game>> GetGamesByFilteredSkipAndTakeCountAsync(int skipCount, int takeCount, int categoryId);

        Task<List<Game>> GetGamesBySkipAndTakeCountAsync(int skipCount, int takeCount);

        Task<Game> GetGameWithIncludeAsync(int id);

        Task<bool> AddRangeAsync(Game game, GameDetail gameDetail);

        Task<bool> CheckGameAsync(Expression<Func<Game, bool>> filter);
    }
}

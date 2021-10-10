using Core.Repository;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IGameDal : IRepository<Game>
    {
        Task<List<Game>> GetGamesByTakeCountAsync(int takeCount);

        Task<List<Game>> GetGamesBySkipAndTakeCountAsync(int skipCount, int takeCount);

        Task<Game> GetGameWithIncludeAsync(int id);

        Task<bool> AddRangeAsync(Game game, GameDetail gameDetail);
    }
}

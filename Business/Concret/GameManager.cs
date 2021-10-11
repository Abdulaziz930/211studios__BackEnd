using Business.Abstract;
using DataAccess.Abstract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concret
{
    public class GameManager : IGameService
    {
        private readonly IGameDal _gameDal;

        public GameManager(IGameDal gameDal)
        {
            _gameDal = gameDal;
        }

        public async Task<bool> AddAsync(Game game)
        {
            await _gameDal.AddAsync(game);

            return true;
        }

        public async Task<bool> AddRangeAsync(Game game, GameDetail gameDetail)
        {
            await _gameDal.AddRangeAsync(game, gameDetail);

            return true;
        }

        public async Task<bool> CheckGameAsync(Expression<Func<Game, bool>> filter)
        {
            return await _gameDal.CheckGameAsync(filter);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _gameDal.DeleteAsync(new Game { Id = id });

            return true;
        }

        public async Task<Game> GetGameAsync(int id)
        {
            return await _gameDal.GetAsync(x => x.IsDeleted == false && x.Id == id);
        }

        public async Task<List<Game>> GetGamesAsync()
        {
            return await _gameDal.GetAllAsync(x => x.IsDeleted == false);
        }

        public async Task<List<Game>> GetGamesAsync(int takeCount)
        {
            return await _gameDal.GetGamesByTakeCountAsync(takeCount);
        }

        public async Task<List<Game>> GetGamesAsync(int skipCount, int takeCount)
        {
            return await _gameDal.GetGamesBySkipAndTakeCountAsync(skipCount, takeCount);
        }

        public async Task<Game> GetGameWithIncludeAsync(int id)
        {
            return await _gameDal.GetGameWithIncludeAsync(id);
        }

        public async Task<bool> UpdateAsync(Game game)
        {
            await _gameDal.UpdateAsync(game);

            return true;
        }
    }
}

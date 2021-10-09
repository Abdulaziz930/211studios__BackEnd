using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IGameService
    {
        Task<Game> GetGameAsync(int id);

        Task<Game> GetGameWithIncludeAsync(int id);

        Task<List<Game>> GetGamesAsync();

        Task<List<Game>> GetGamesAsync(int takeCount);
        
        Task<List<Game>> GetGamesAsync(int skipCount, int takeCount);

        Task<bool> AddAsync(Game game);

        Task<bool> UpdateAsync(Game game);

        Task<bool> DeleteAsync(int id);
    }
}

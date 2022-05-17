using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerOfWitch.Interfaces;
using TowerOfWitch.Models;

namespace TowerOfWitch.Services
{
    public class PlayersService : IPlayersService
    {
        PlayersDbContext dbContext = null;
        public PlayersService()
        {
            dbContext = new PlayersDbContext();
        }

        public async Task<int> RegisterPlayerAsync(Player player)
        {
            if(player.UserName == null) // User name is null
            {
                return 3;
            }
            try
            {
                List<Player> players = dbContext.Players.ToList();
                if (players.Count == 0 || players.Where(el => el.UserId == player.UserId).FirstOrDefault() == null)
                {
                    dbContext.Add(player);
                    await dbContext.SaveChangesAsync();
                    return 1;
                } else
                {
                    return 2;
                }
            } catch
            {
                return 0;
            }
        }

        public Task<Player> FindPlayerByUserNameAsync(string UserName)
        {
            throw new NotImplementedException();
        }

        public Task<Player> GetPlayerAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePlayerAsync(Player player)
        {
            throw new NotImplementedException();
        }

        

        ~PlayersService()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}

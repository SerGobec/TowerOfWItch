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

        public Player FindPlayerByUserName(string UserName)
        {
            return dbContext.Players.Where(el => el.UserName == UserName).FirstOrDefault();
        }

        public Player GetPlayerByID(long id)
        {
            return dbContext.Players.Where(el => el.UserId == id).FirstOrDefault();
        }

        public Task<int> UpdatePlayerAsync(Player player)
        {
             dbContext.Update(player);
             return dbContext.SaveChangesAsync();
        }

        public bool IsRegistered(long UserId)
        {
            Player player = dbContext.Players.Where(el=> el.UserId == UserId).FirstOrDefault();
            return player != null;
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

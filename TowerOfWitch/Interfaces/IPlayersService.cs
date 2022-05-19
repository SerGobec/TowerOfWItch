using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerOfWitch.Models;

namespace TowerOfWitch.Interfaces
{
    public interface IPlayersService
    {
        public Task<int> RegisterPlayerAsync(Player player);
        public Player GetPlayerByID(long id);
        public Player FindPlayerByUserName(string UserName);
        public Task<int> UpdatePlayerAsync(Player player);
        public bool IsRegistered(long UserId);
    }
}

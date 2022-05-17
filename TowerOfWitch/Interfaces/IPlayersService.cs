using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerOfWitch.Models;

namespace TowerOfWitch.Interfaces
{
    interface IPlayersService
    {
        public Task<int> RegisterPlayerAsync(Player player);
        public Task<Player> GetPlayerAsync(long id);
        public Task<Player> FindPlayerByUserNameAsync(string UserName);
        public Task<bool> UpdatePlayerAsync(Player player);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerOfWitch.Models;

namespace TowerOfWitch.Interfaces
{
    interface IGameService<T>
    {
        public void CreateGame(T t);
        public void AcceptGame(T t);
        public void DoMove(T t, int n);
        public Player CheckForWiner(GameModel game);
        public void Resign(T t);
        public void Reject(T t);
        
    }
}

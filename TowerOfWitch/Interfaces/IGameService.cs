using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfWitch.Interfaces
{
    interface IGameService<T>
    {
        public void CreateGame(T t);
        public void AcceptGame(T t);
        public void DoMove(T t);
        public void CheckForWiner();
        public void Resign(T t);
        public void Reject(T t);
        
    }
}

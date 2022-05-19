using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfWitch.Interfaces
{
    public interface IPurchasesService
    {
        public Task AddSymbolToUser(long UserId, int SumbolId);
        public List<int> GetAvailableSymbols(long UserId);
        public uint GetBalance(long UserId);
    }
}

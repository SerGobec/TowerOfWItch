using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerOfWitch.Interfaces;
using TowerOfWitch.Models;

namespace TowerOfWitch.Services
{
    class PurchasesService : IPurchasesService
    {
        PlayersDbContext dbContext = null;
        public PurchasesService()
        {
            dbContext = new PlayersDbContext();
        }

        public async Task AddSymbolToUser(long UserId, int SumbolId)
        {
            UserSymbolPair userSymbolPair = new UserSymbolPair
            {
                UserId = UserId,
                SymbolIndex = SumbolId
            };
            dbContext.Add(userSymbolPair);
            await dbContext.SaveChangesAsync();
        }

        public List<int> GetAvailableSymbols(long UserId)
        {
            return dbContext.UserSymbolPairs.Where(el => el.UserId == UserId).Select(el => el.SymbolIndex).ToList();
        }

        public uint GetBalance(long UserId)
        {
            return dbContext.Players.Where(el => el.UserId == UserId).Select(el => el.Coins).FirstOrDefault();
        }
    }
}

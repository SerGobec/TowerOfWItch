using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfWitch.Interfaces
{
    public interface IShopService<T>
    {
        public Task MoneyAsync(T t);
        public Task ShopAsync(T t);
        public Task Buy(T t);
        public Task AvailableSymbol(T t);
        public Task SetSymbol(T t);
        public Task AddSymbolForUserAsync(T update, int SymbolId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerOfWitch.Models;

namespace TowerOfWitch.Interfaces
{
    interface INotificator
    {
        public Task NotificateWinnerAsync(Player pl);
        public Task NotificateLoserAsync(Player pl);
        public Task NotificateResignedAsync(Player pl);
        public Task NotificateDrawAsync(Player pl);
    }
    
}

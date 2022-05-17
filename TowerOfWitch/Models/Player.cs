using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfWitch.Models
{
    public class Player
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public int CountOfGame { get; set; }
        public int WinGame { get; set; }
        public uint Coins { get; set; }
        public bool InGame { get; set; }
        public int SymbolCode { get; set; }
    }
}

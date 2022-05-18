using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerOfWitch.Services;

namespace TowerOfWitch.Models
{
    public class GameModel
    {
        
        public List<Player> Players { get; }
        public byte[,] Area = new byte[7, 7];
        public byte Turn { get; set; }
        public bool Accepted { get; set; }
        public byte MoveCounter { get; set; }

        public GameModel(Player pl1, Player pl2)
        {
            MoveCounter = 0;
            Players = new List<Player>();
            Players.Add(pl1);
            Players.Add(pl2);
            Accepted = false;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Area[i, j] = 0;
                }
            }
        }
        public string WriteArea()
        {
            string result = "--------------------------------\n";
            for(int i = 0;i < 7; i++)
            {
                result += "| ";
                for(int j = 0;j < 7; j++)
                {
                    result += SymbolService.GetSymbolByCode(Area[i,j]);
                }
                result += " |\n";
            }
            result += "--------------------------------\n";
            result += "| 1⃣2⃣3⃣4⃣5⃣6⃣7⃣ |";
            return result;
        }
    }
}

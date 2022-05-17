using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfWitch.Models
{
    public class GameModel
    {
        public List<Player> Players { get; set; }
        public int[,] Area = new int[7, 7];
        public byte Turn { get; set; }

        public string WriteArea()
        {
            string result = "-----------";
            for(int i = 0;i < 7; i++)
            {
                result += "| ";
                for(int j = 0;j < 7; j++)
                {
                    result += Area[i, j];
                }
                result += " |\n";
            }
            result += "-----------";
            return result;
        }
    }
}

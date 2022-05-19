using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfWitch.Services
{
    static class SymbolService
    {
        public static Dictionary<int, string> Symbols = new Dictionary<int, string>
        {
            {0,  "⬜"},
            {1, "🔶" },
            {2, "🔶" },
            {3, "⭐" },
            {4, "⭕" },
            {5, "⚽" },
            {6, "⚾" },
            {7, "💣" },
            {8, "💩" },
            {9, "📀" },
            {10, "💿" },
            {11, "📛" },
            {12, "🔞" },
            {13, "🌿" }
        };

        public static Dictionary<int, int> Prices = new Dictionary<int, int>
        {
            {1, 0 },
            {2, 0 },
            {3, 5 },
            {4, 5 },
            {5, 5 },
            {6, 10 },
            {7, 20 },
            {8, 20 },
            {9, 25 },
            {10, 25 },
            {11, 25 },
            {12, 50 },
            {13, 50 }
        };
        public static string GetSymbolByCode(int code)
        {
            if (Symbols.ContainsKey(code))
            {
                return Symbols[code];
            }
            return "X";
        }
        public static int GetCodeBySymbol(string symb)
        {
            if (Symbols.ContainsValue(symb))
            {
                return Symbols.FirstOrDefault(el => el.Value == symb).Key;
            }
            return -1;
        }
    }
}

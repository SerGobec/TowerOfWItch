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

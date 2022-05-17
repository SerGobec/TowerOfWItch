using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TowerOfWitch.Interfaces;
using TowerOfWitch.Models;

namespace TowerOfWitch.Services
{
    public class GameService : IGameService<Update>
    {
        List<GameModel> _games = new List<GameModel>();
        TelegramBotClient _bot;
        PlayersService playersDb;

        public GameService(ITelegramBotClient bot, PlayersService players)
        {
            this._bot = (TelegramBotClient)bot;
            this.playersDb = players;
        }

        public void AcceptGame(Update update)
        {
            throw new NotImplementedException();
        }

        public void CheckForWiner(Update update)
        {
            throw new NotImplementedException();
        }

        public async void CreateGame(Update update)
        {
           // await bot.SendTextMessageAsync();
        }

        public void DoMove(Update update)
        {
            throw new NotImplementedException();
        }
    }
}

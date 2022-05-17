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
        PlayersService playersService;

        public GameService(ITelegramBotClient bot, PlayersService players)
        {
            this._bot = (TelegramBotClient)bot;
            this.playersService = players;
        }

        public void AcceptGame(Update update)
        {
            Player player1 = playersService.FindPlayerByUserName(update.Message.Text.Split()[1]);
            Player player2 = playersService.GetPlayerByID(update.Message.From.Id);
            if(player1 == null || player1.InGame)
            {
                _bot.SendTextMessageAsync(update.Message.From.Id, "Your opponent is playing now!" +
                    "\nOr he was killed by another witcher👥");
            }
            if(player2 == null)
            {
                _bot.SendTextMessageAsync(update.Message.From.Id, "We can`t find you in system🌚" +
                    "\nMay be you forgot to register?" +
                    "\nWrite /reg");
            }
            if (player2.InGame)
            {
                _bot.SendTextMessageAsync(update.Message.From.Id, "You are alredy playing🐀");
            }
            GameModel game = _games.Where(el => el.Players.Contains(player1) &&
                                                el.Players.Contains(player2)).FirstOrDefault();
            if(game == null)
            {
                _bot.SendTextMessageAsync(update.Message.From.Id, "We can`t find challange from your opponent🌚" +
                    "\nYou can create challenge" +
                    "\nWrite " +
                    "\n/create OpponentUserName");
            }
            game.accepted = true;
            Random rnd = new Random();
            game.Turn = (byte)rnd.Next(0,1);
            for(int i = 0;i < 2; i++)
            {
                _bot.SendTextMessageAsync(game.Players[i].UserId, "Game has been started!!!" +
                    "\n " + SymbolService.GetSymbolByCode(player1.SymbolCode)+ " "+ player1.UserName +
                    " --- " + player2.UserName + " " + SymbolService.GetSymbolByCode(player2.SymbolCode)+ "" +
                    "\n" + game.WriteArea() +
                    "\nThe first player to act is: " + game.Players[game.Turn].UserName); 
            }
                
        }

        public void CheckForWiner()
        {
            throw new NotImplementedException();
        }

        public async void CreateGame(Update update)
        {
            Player player1 = playersService.GetPlayerByID(update.Message.From.Id);
            if(player1 == null)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "You are not registered!"+
                    "Or someone has stolen your data...");
                return;
            }
            if (player1.InGame)
            {
                await _bot.SendTextMessageAsync(player1.UserId, "You are already in game!");
                return;
            }
            Player player2 = playersService.FindPlayerByUserName(update.Message.Text.Split()[1]);
            if(player2 == null)
            {
                await _bot.SendTextMessageAsync(player1.UserId, "Your opponent not found...\n" +
                    "Or hasn`t alredy registered in system💢");
                return;
            }
            if (player2.InGame)
            {
                await _bot.SendTextMessageAsync(player1.UserId, "Your opponent are playing now.");
                return;
            }
            GameModel game = new GameModel(player1, player2);
            _games.Add(game);
            await _bot.SendTextMessageAsync(player1.UserId, "OK. I will send him your chalange, master...🗿 ");
            await _bot.SendTextMessageAsync(player2.UserId, player1.UserName + " want you to fight with him 🔪" +
                "\nIf you accept chalange, write me:" +
                "\n/accept " + player1.UserName +
                "\nIf you don`t wont to play now, write:" +
                "\n/reject " + player1.UserName);
            return;
        }

        public void DoMove(Update update)
        {
            //
        }

        public void Reject(Update t)
        {
            //
        }

        public void Resign(Update t)
        {
            //
        }
    }
}

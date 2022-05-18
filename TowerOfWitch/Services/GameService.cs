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

        public async void AcceptGame(Update update)
        {
            Player player1 = playersService.FindPlayerByUserName(update.Message.Text.Split()[1]);
            Player player2 = playersService.GetPlayerByID(update.Message.From.Id);
            if(player1 == null || player1.InGame)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "Your opponent is playing now!" +
                    "\nOr he was killed by another witcher👥");
                return;
            }
            if(player2 == null)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "We can`t find you in system🌚" +
                    "\nMay be you forgot to register?" +
                    "\nWrite /reg");
                return;
            }
            if (player2.InGame)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "You are alredy playing🐀");
                return;
            }
            GameModel game = _games.Where(el => el.Players.Contains(player1) &&
                                                el.Players.Contains(player2)).FirstOrDefault();
            if(game == null)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "We can`t find challange from your opponent🌚" +
                    "\nYou can create challenge" +
                    "\nWrite " +
                    "\n/create OpponentUserName");
                return;
            }
            game.accepted = true;
            Random rnd = new Random();
            game.Turn = (byte)rnd.Next(0,1);
            for(int i = 0;i < 2; i++)
            {
                await _bot.SendTextMessageAsync(game.Players[i].UserId, "Game has been started!!!" +
                    "\n " + SymbolService.GetSymbolByCode(player1.SymbolCode)+ " "+ player1.UserName +
                    " --- " + player2.UserName + " " + SymbolService.GetSymbolByCode(player2.SymbolCode)+ "" +
                    "\n" + game.WriteArea() +
                    "\nThe first player to act is: " + game.Players[game.Turn].UserName); 
            }
                
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

        public async void Reject(Update update)
        {
            Player player2 = playersService.FindPlayerByUserName(update.Message.Text.Split()[1]);
            Player player1 = playersService.GetPlayerByID(update.Message.From.Id);
            if (player2 == null)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "We can`t find your opponent..." +
                    "\nOr he was killed by another witcher👥");
                return;
            }
            if (player1 == null)
            {  
                await _bot.SendTextMessageAsync(update.Message.From.Id, "We can`t find you in system🌚" +
                    "\nMay be you forgot to register?" +
                    "\nWrite /reg");
                return;
            }
            GameModel game = _games.Where(el => el.Players.Contains(player1) &&
                                           el.Players.Contains(player2)).FirstOrDefault();
            if(game == null)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "We can`t find this game🌚" +
                    "\nSo.. You are free for now🚶");
                return;
            }
            if (game.accepted)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "You can`t reject game that was alreade accepted..." +
                    "\nBut. You can resign🙈" +
                    "\nWrite /resign");
                return;
            }

            await _bot.SendTextMessageAsync(update.Message.From.Id, "Game was rejected.");
            await _bot.SendTextMessageAsync(player2.UserId, player1.UserName + " rejected your challange.");
            _games.Remove(game);
        }

        public void Resign(Update t)
        {
            //
        }

        public Player CheckForWiner(GameModel game)
        {
            string[,] area = game.Area;
            for(int i = 0; i < 7; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if(area[i, j] == area[i, j + 1] &&
                       area[i, j] == area[i, j + 2] &&
                       area[i, j] == area[i, j + 3] &&
                       area[i, j] != "⬜")
                    {
                        int key = SymbolService.GetCodeBySymbol(area[i,j]);
                        return game.Players.Where(el => el.SymbolCode == key).FirstOrDefault();
                    }
                    if(area[j, i] == area[j + 3, i] &&
                       area[j, i] == area[j + 1, i] &&
                       area[j, i] == area[j + 2, i] &&
                       area[j, i] != "⬜")
                    {
                        int key = SymbolService.GetCodeBySymbol(area[j, i]);
                        return game.Players.Where(el => el.SymbolCode == key).FirstOrDefault();
                    }
                }
            }
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0;j < 4; j++)
                {
                    if (area[i, j] == area[i + 1, j + 1] &&
                       area[i, j] == area[i + 2, j + 2] &&
                       area[i, j] == area[i + 3, j+ 3] &&
                       area[i, j] != "⬜")
                    {
                        int key = SymbolService.GetCodeBySymbol(area[j, i]);
                        return game.Players.Where(el => el.SymbolCode == key).FirstOrDefault();
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 3; j < 7; j++)
                {
                    if (area[i, j] == area[i + 1, j - 1] &&
                       area[i, j] == area[i + 2, j - 2] &&
                       area[i, j] == area[i + 3, j - 3] &&
                       area[i, j] != "⬜")
                    {
                        int key = SymbolService.GetCodeBySymbol(area[j, i]);
                        return game.Players.Where(el => el.SymbolCode == key).FirstOrDefault();
                    }
                }
            }

            return null;
        }

        public async void DoMove(Update update, int num)
        {
            Player player = playersService.GetPlayerByID(update.Message.From.Id);
            if (player == null)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "We can`t find you in system🌚" +
                    "\nMay be you forgot to register?" +
                    "\nWrite /reg");
                return;
            }
            GameModel game = _games.Where(el => el.accepted && el.Players.Contains(player)).FirstOrDefault();
            if(game == null)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "You are`t playing now." +
                    "You are free witcher. 👀");
                return;
            }
            if(num < 1 || num > 7)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "Column must be btw 1 and 7👀");
                return;
            }
            if(game.Players.IndexOf(player) != game.Turn)
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "That is not your turn🐼");
                return;
            }
            if(game.Area[0, num - 1] != "⬜")
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "You can`t put your figure higher👅");
                return;
            }

            Player winner = null;
            for (int i = 6; i >= 0; i--)
            {
                if(game.Area[i, num - 1] == "⬜")
                {
                    game.Area[i, num - 1] = SymbolService.GetSymbolByCode(player.SymbolCode);
                    winner = CheckForWiner(game);
                    if (winner != null)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            await _bot.SendTextMessageAsync(game.Players[j].UserId, winner.UserName + "is winner!");
                            game.Players[j].InGame = false;
                            await playersService.UpdatePlayerAsync(game.Players[j]);
                        }
                        _games.Remove(game);
                        break;
                    }
                    switch (game.Turn)
                    {
                        case 1:
                            game.Turn = 0;
                            break;
                        case 0:
                            game.Turn = 1;
                            break;
                    }
                    break;
                }
            }
            for (int i = 0; i < 2; i++)
            {
                await _bot.SendTextMessageAsync(game.Players[i].UserId, game.WriteArea());
            }
            
        }
    }
}

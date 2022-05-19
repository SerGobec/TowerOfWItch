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
    public class ShopService : IShopService<Update>
    {
        Random rnd = new Random();
        TelegramBotClient _bot;
        IPlayersService _playersService;
        IPurchasesService _purchasesService;

        public ShopService(ITelegramBotClient bot, IPlayersService pS, IPurchasesService pchS)
        {
            this._bot = (TelegramBotClient)bot;
            this._playersService = pS;
            this._purchasesService = pchS;

        }

        public async Task AvailableSymbol(Update update)
        {
            Player pl = _playersService.GetPlayerByID(update.Message.From.Id);
            List<int> symbols = _purchasesService.GetAvailableSymbols(update.Message.From.Id);
            string answer = "Chosen symbol: " + SymbolService.GetSymbolByCode(pl.SymbolCode) +
                "\nFor now you can chose those symbols:\n\n";
            foreach(int index in symbols)
            {
                answer += SymbolService.GetSymbolByCode(index) + "  id: " + index + "\n\n";
            }
            answer += "ENG: If you want chose somewich, write /set SymbolId .For example:\n" +
                "<b><i>/set 3</i></b>\n";
            answer += "UKR: Якщо ви хочете обрати будь який з доступних символів, пишіть /set SymbolId .Наприклад:\n" +
                "<b><i>/set 3</i></b>\n";
            answer += "You can buy more🤑\n" +
                "Write /shop";
            await _bot.SendTextMessageAsync(update.Message.From.Id, answer, Telegram.Bot.Types.Enums.ParseMode.Html);
        }

        public async Task Buy(Update update)
        {
            byte index;
            bool isNumber = byte.TryParse(update.Message.Text.Split()[1], out index);
            if (isNumber)
            {
                if (!SymbolService.Prices.ContainsKey(index))
                {
                    await _bot.SendTextMessageAsync(update.Message.From.Id, "There isn`t this index in shop😳");
                    return;
                }
                if (!_purchasesService.GetAvailableSymbols(update.Message.From.Id).Contains(index))
                {
                    Player pl = _playersService.GetPlayerByID(update.Message.From.Id);
                    if (pl != null)
                    {
                        if (pl.Coins >= SymbolService.Prices[index])
                        {
                            pl.SymbolCode = index;
                            pl.Coins -= (uint)SymbolService.Prices[index];
                            await _purchasesService.AddSymbolToUser(pl.UserId, index);
                            await _playersService.UpdatePlayerAsync(pl);
                            await _bot.SendTextMessageAsync(update.Message.From.Id, "Symbol succesfully changed🤩");
                        }
                        else
                        {
                            await _bot.SendTextMessageAsync(update.Message.From.Id, "Not anought money( You can earn them just playing😋");
                        }
                        
                        
                    }
                    else
                    {
                        await _bot.SendTextMessageAsync(update.Message.From.Id, "We can`t find your account🤯");
                    }
                }
                else
                {
                    await _bot.SendTextMessageAsync(update.Message.From.Id, "Symbol is already available for you😎");
                }
            }
            else
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "You have to wrire /buy + [ID of symbol]. Where id is number🤖");
            }
        }

        public async Task MoneyAsync(Update update)
        {
            if (!_playersService.IsRegistered(update.Message.From.Id))
            {
                await _playersService.RegisterPlayerAsync(new Models.Player()
                {
                    SymbolCode = (byte)rnd.Next(1, SymbolService.Symbols.Count + 1),
                    Coins = 0,
                    CountOfGame = 0,
                    UserId = update.Message.From.Id,
                    UserName = update.Message.From.Username,
                    Name = update.Message.From.FirstName,
                    InGame = false,
                    WinGame = 0
                });
            }
            uint coins = _purchasesService.GetBalance(update.Message.From.Id);
            await _bot.SendTextMessageAsync(update.Message.From.Id, "Well master, for now we have: " +
                "\n "+ coins + "🍊");
        }

        public async Task SetSymbol(Update update)
        {
            byte index;
            bool isNumber = byte.TryParse(update.Message.Text.Split()[1], out index);
            if (isNumber)
            {
                if (_purchasesService.GetAvailableSymbols(update.Message.From.Id).Contains(index))
                {
                    Player pl = _playersService.GetPlayerByID(update.Message.From.Id);
                    if(pl != null)
                    {
                        pl.SymbolCode = index;
                        await _playersService.UpdatePlayerAsync(pl);
                        await _bot.SendTextMessageAsync(update.Message.From.Id, "Symbol succesfully changed🤩");
                    } else
                    {
                        await _bot.SendTextMessageAsync(update.Message.From.Id, "We can`t find your account🤯");
                    }
                } else
                {
                    await _bot.SendTextMessageAsync(update.Message.From.Id, "Symbol with this id unavailable for you😥");
                }
            } else
            {
                await _bot.SendTextMessageAsync(update.Message.From.Id, "You have to wrire /set + [ID of symbol]. Where id is number🤖");
            }
        }

        public async Task AddSymbolForUserAsync(Update update, int SymbolId)
        {
            if (!_purchasesService.GetAvailableSymbols(update.Message.From.Id).Contains(SymbolId))
            {
                await _purchasesService.AddSymbolToUser(update.Message.From.Id, SymbolId);
            }
        }

        public async Task ShopAsync(Update update)
        {
            string answ = "🧙🏻‍♂️: Wellcome my dear!!!" +
                "\nFor today we have for you this stuff:\n";
            foreach(var pair in SymbolService.Prices)
            {

                answ += "\n|" + SymbolService.GetSymbolByCode(pair.Key) +
                    "\n|Price: <b>" + pair.Value + "🍊</b>" +
                    "\n|ID: " + pair.Key;
                answ += "\n";
            }
            answ += "\n\n";
            Player pl = _playersService.GetPlayerByID(update.Message.From.Id);
            if(pl != null)
            {
                answ += "|<b>You have: " + pl.Coins + "</b>🍊|\n";
            }

            answ += 
                "ENG: if you want to buy somewich, write /buy and ID of stuff. For Example:" +
                "\n<b><i>/buy 4</i></b>" +
                "\nUKR: Якщо ви хочете купити якийсь з товарів, напишіть /buy та код(id) товару. Наприклад:" +
                "\n<b><i>/buy 4</i></b>";
            await _bot.SendTextMessageAsync(update.Message.From.Id, answ, Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
}

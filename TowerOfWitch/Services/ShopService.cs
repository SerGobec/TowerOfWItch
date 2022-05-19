using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TowerOfWitch.Interfaces;

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

        public Task AvailableSymbol(Update update)
        {
            throw new NotImplementedException();
        }

        public Task Buy(Update t)
        {
            throw new NotImplementedException();
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

        public Task SetSymbol(Update t)
        {
            throw new NotImplementedException();
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
                    "\n|Price: <b>" + pair.Value + "</b>" +
                    "\n|ID: " + pair.Key;
                answ += "\n";
            }
            answ += "ENG: if you want to buy somewich, write /buy and ID of stuff. For Example:" +
                "\n<b><i>/buy 4</i></b>" +
                "\nUKR: Якщо ви хочете купити якийсь з товарів, напишіть /buy та код(id) товару. Наприклад:" +
                "\n<b><i>/buy 4</i></b>";
            await _bot.SendTextMessageAsync(update.Message.From.Id, answ, Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
}

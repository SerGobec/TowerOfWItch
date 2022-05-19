using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using TowerOfWitch.Interfaces;
using TowerOfWitch.Models;
using TowerOfWitch.Services;
using Telegram.Bot.Types.Enums;

namespace TowerOfWitch
{
    class Program
    {
        static ITelegramBotClient bot;
        static PlayersService playersService;
        static PurchasesService purchasesService;
        static IGameService<Update> gameService;
        static IShopService<Update> shopService;
        static Random rnd = new Random();
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                

                Console.WriteLine("From:     " + update.Message.From.Username + "  -->  " + update.Message.Text);
                var message = update.Message;
                //--------------------------------
                // Automatic registration
                /*if (!playersService.IsRegistered(update.Message.From.Id))
                {

                    Random rnd = new Random();
                    byte symb = (byte)rnd.Next(1, SymbolService.Symbols.Count + 1);
                    Player player = new Player
                    {
                        UserId = message.From.Id,
                        Name = message.From.FirstName,
                        UserName = message.From.Username,
                        CountOfGame = 0,
                        WinGame = 0,
                        InGame = false,
                        Coins = 5,
                        SymbolCode = symb
                    };
                    int result = await playersService.RegisterPlayerAsync(player);
                    switch (result)
                    {
                        case 1:
                            await botClient.SendTextMessageAsync(message.From.Id, "You are registered now!\n" +
                                "Wellcome to family witcher 👤");
                            await shopService.AddSymbolForUserAsync(update, symb);
                            break;
                        case 2:
                            await botClient.SendTextMessageAsync(message.From.Id, "You`ve been already registered!\n" +
                                "Brother? 👤");
                            break;
                        case 3:
                            await botClient.SendTextMessageAsync(message.From.Id, "We have tried to register you, but you must create username in telegram.\n" +
                                "Or... You want us to call you bi-bu-bip..? 👤");
                            break;
                        default:
                            await botClient.SendTextMessageAsync(message.From.Id, "Something wrong...\n" +
                                "I can`t read some symbol...\n" +
                                "may be wrong magic book...👤");
                            break;
                    }
                }*/
                //--------------------------------
                int len = message.Text.Split().Length;
                if(len == 1)
                {
                    if (message.Text.ToLower() == "/start")
                    {
                        await botClient.SendTextMessageAsync(message.From.Id, "Wellcome witcher!" +
                            "\nTelegram game Tower of witcher for everyone!." +
                            "\nWelcome.👤" +
                            "\n---💀----------" +
                            "\nFor register in base write /reg." +
                            "\nThis will let your friend call you for a game." +
                            "\n---------👽----" +
                            "\nFor ..");
                        return;
                    }

                    if (message.Text.ToLower() == "/reg")
                    {
                        Random rnd = new Random();
                        byte symb = (byte)rnd.Next(1, SymbolService.Symbols.Count + 1);
                        Player player = new Player
                        {
                            UserId = message.From.Id,
                            Name = message.From.FirstName,
                            UserName = message.From.Username,
                            CountOfGame = 0,
                            WinGame = 0,
                            InGame = false,
                            Coins = 5,
                            SymbolCode = symb
                        };
                        int result = await playersService.RegisterPlayerAsync(player);
                        switch (result)
                        {
                            case 1:
                                await botClient.SendTextMessageAsync(message.From.Id, "You are registered now!\n" +
                                    "Wellcome to family witcher 👤" +
                                    "\nYour first symbol for game: " + SymbolService.GetSymbolByCode(symb));
                                await shopService.AddSymbolForUserAsync(update, symb);
                                break;
                            case 2:
                                await botClient.SendTextMessageAsync(message.From.Id, "You`ve been already registered!\n" +
                                    "Brother? 👤");
                                break;
                            case 3:
                                await botClient.SendTextMessageAsync(message.From.Id, "You must create username in telegram.\n" +
                                    "Or... You want us to call you bi-bu-bip..? 👤");
                                break;
                            default:
                                await botClient.SendTextMessageAsync(message.From.Id, "Something wrong...\n" +
                                    "I can`t read some symbol...\n" +
                                    "may be wrong magic book...👤");
                                break;
                        }

                        return;
                    }
                    if (message.Text.ToLower() == "/resign")
                    {
                        gameService.Resign(update);
                        return;
                    }
                    if (message.Text.ToLower() == "/create")
                    {
                        await bot.SendTextMessageAsync(message.From.Id, "ENG:" +
                            "\nIf you want send challenge to your friend, after /create write his nickname. For example:" +
                            "\n<b><i>/create Grey_P</i></b>" +
                            "\nUKR:" +
                            "\nЯкщо ви хочете викликати друга на поєдинок, після /create потрібно" +
                            "написати його username а телеграмі. Наприклад:\n" +
                            "<b><i>/create Grey_P</i></b>", ParseMode.Html);
                        return;
                    }
                    if (message.Text.ToLower() == "/accept")
                    {
                        await bot.SendTextMessageAsync(message.From.Id, "ENG:" +
                            "\nIf you accept  challenge to your friend, after /accept write his nickname. For example:" +
                            "\n<b><i>/accept Grey_P</i></b>" +
                            "\nUKR:" +
                            "\nЯкщо ви хочете прийняти виклик на поєдинок, після /accept потрібно" +
                            "написати його username а телеграмі. Наприклад:" +
                            "\n<b><i>/accept Grey_P</i></b>", ParseMode.Html);
                        return;
                    }
                    if (message.Text.ToLower() == "/reject")
                    {
                        await bot.SendTextMessageAsync(message.From.Id, "ENG:" +
                            "\nIf you reject challenge of your friend, after /reject write his nickname. For example:" +
                            "\n<b><i>/reject Grey_P</i></b>" +
                            "\nUKR:" +
                            "\nЯкщо ви хочете відхилити виклик на поєдинок, після /accept потрібно" +
                            "написати його username а телеграмі. Наприклад:" +
                            "\n<b><i>/reject Grey_P</i></b>", ParseMode.Html);
                        return;
                    }
                    if (message.Text.ToLower() == "/money")
                    {
                        if (playersService.IsRegistered(message.From.Id))
                        {
                            await shopService.MoneyAsync(update); 
                        } else
                        {
                            await botClient.SendTextMessageAsync(message.From.Id, "Some function available only for registered users..." +
                                "\nJust write /reg 😌");
                        }
                        return;
                    }
                    if (message.Text.ToLower() == "/shop")
                    {
                        if (playersService.IsRegistered(message.From.Id))
                        {
                            await shopService.ShopAsync(update);
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(message.From.Id, "Some function available only for registered users..." +
                                "\nJust write /reg 😌");
                        }
                        return;

                    }
                        int number;
                    bool isNumber = int.TryParse(message.Text, out number);
                    if (isNumber)
                    {
                        gameService.DoMove(update, number);
                        return;
                    }
                }
                if(len == 2)
                {
                    if(message.Text.Split()[0].ToLower() == "/create")
                    {
                        gameService.CreateGame(update);
                        return;
                    }
                    if (message.Text.Split()[0].ToLower() == "/accept")
                    {
                        gameService.AcceptGame(update);
                        return;
                    }
                    if (message.Text.Split()[0].ToLower() == "/reject")
                    {
                        gameService.Reject(update);
                        return;
                    }
                    if (message.Text.Split()[0].ToLower() == "/buy")
                    {
                        await shopService.Buy(update);
                        return;
                    }
                }
                await botClient.SendTextMessageAsync(message.From.Id, len + " words🤨");
            } 
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            string token = null;
            using (StreamReader reader = new StreamReader("token.txt"))
            {
                
                while (reader.Peek() != -1)
                {
                    token = reader.ReadLine();
                }
                
            }
            Console.WriteLine(token);
            if(token == null)
            {
                Console.WriteLine("Token file not found :(");
                return;
            }
            bot = new TelegramBotClient(token);
            playersService = new PlayersService();
            purchasesService = new PurchasesService();

            shopService = new ShopService(bot, playersService, purchasesService);
            gameService = new GameService(bot, playersService);

            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}

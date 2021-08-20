using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramAspBot.Models.Commands
{
    public class TopListCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/top" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, TopListCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.First(p => p.TelegramId == message.From.Id);

            var currentSeason = dbContext.SeasonStats.First(p => !p.IsEnded);

            var split = message.Text.Split("_");
            if (split.Length == 1)
            {
                await telegramBot.SendTextMessageAsync(character.TelegramId, $"Список топов по % пойманого /top_percent\n" +
                                                                             $"Список топов по пойманой рыбе /top_caught\n" +
                                                                             $"Сезонные очки #{currentSeason.Number} /top_season");
            }
            else
            {
                var topSystem = new TopPlayersSystem(character);
                switch (split[1])
                {
                    case "percent":
                    {
                        await telegramBot.SendTextMessageAsync(character.TelegramId, topSystem.GetTopListByPercentCatches().ToString(), ParseMode.Html);
                    }
                        break;
                    case "caught":
                    {
                        await telegramBot.SendTextMessageAsync(character.TelegramId, topSystem.GetTopListByCatchedFish().ToString(), ParseMode.Html);
                    }
                        break;
                    case "season":
                    {
                        await telegramBot.SendTextMessageAsync(character.TelegramId, topSystem.GetTopByCurrentSeasonPoint().ToString(), ParseMode.Html);
                    }
                        break;
                }
            }
            
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
            {
                return false;
            }
            
            using var dbContext = new ApplicationContext();
            var userId = message.From.Id;
            if (message.Chat.Id != userId)
            {
                return false;
            }

            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == userId);
            
            foreach (var command in Name)
            {
                if (message.Text.Contains(command) && character != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
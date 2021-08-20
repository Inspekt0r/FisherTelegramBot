using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.Commands
{
    public class EndAndStartSeasonCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/newseason"};
        private const int AdminId = 137968009;
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, EndAndStartSeasonCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();
            var activeSeason = dbContext.SeasonStats.FirstOrDefault(p => !p.IsEnded);
            
            if (activeSeason == null)
            {
                activeSeason = new Season()
                {
                    Month = DateTime.UtcNow.Month,
                    Number = 1
                };
                dbContext.Add(activeSeason);
                await dbContext.SaveChangesAsync();
                await telegramBot.SendTextMessageAsync(AdminId,
                    $"Сезона раньше не было, создал новый {activeSeason.Number}");
                return;
            }
            
            await telegramBot.SendTextMessageAsync(AdminId, 
                $"Новый месяц, пора считать достижения игроков в сезоне {activeSeason.Number}");
                
            activeSeason.IsEnded = true;
            var newSeason = new Season {Month = DateTime.UtcNow.Month, Number = activeSeason.Number + 1};

            var allPlayers = dbContext.Characters.Where(p => p.IsSetupNickname && !p.Banned).ToList();
            var orderedBySeasonPoints = allPlayers.OrderByDescending(p => p.SeasonPoints).ToList();
                
            foreach (var character in allPlayers)
            {
                var giftSystem = new SeasonGifts(character, activeSeason, orderedBySeasonPoints);
                try
                {
                    await telegramBot.SendTextMessageAsync(character.TelegramId, giftSystem.GetGiftForPlayer().ToString(), ParseMode.Html);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            dbContext.Add(newSeason);
            var oldSeason = dbContext.SeasonStats.First(p => p.Id == activeSeason.Id);
            oldSeason.IsEnded = true;
                
            await dbContext.SaveChangesAsync();
                
            await telegramBot.SendTextMessageAsync(AdminId, 
                $"Я посчитал, новый сезон: {newSeason.Number}");
        }

        public bool Contains(Message message)
        {
            return false;
        }
    }
}
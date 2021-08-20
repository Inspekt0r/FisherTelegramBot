using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramAspBot.Job;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.Commands
{
    public class GenerateFishProbability : ICommand
    {
        private readonly FishGenerator _fishGenerator = new FishGenerator();
        private readonly JobManager _jobManager;

        public GenerateFishProbability(JobManager jobManager)
        {
            _jobManager = jobManager;
        }

        public List<string> Name { get; } = new List<string>() { "/probability" };
        
        private const int AdminId = 137968009;
        
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, GenerateFishProbability>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            var jobFish = new JobFunction(AdminId, false, _jobManager, true);
            
            for (var i = 0; i < 10000; i++)
            {
                jobFish.GenerateTextForFishing();
            }

            var sb = new StringBuilder();

            sb.AppendLine($"Результат симуляции:");

            var commonCount = 0;
            var uncommonCount = 0;
            var rareCount = 0;
            var eliteCount = 0;
            var mythicalCount = 0;
            var legendaryCount = 0;
            var uncatched = 0;
            var combo = 0;
            var herbi = 0;
            var carni = 0;

            foreach (var fishTest in jobFish.TestFishes)
            {
                if (fishTest.IsCatch)
                {
                    switch (fishTest.Fish.Rarity)
                    {
                        case Rarity.Common:
                            commonCount++;
                            break;
                        case Rarity.Uncommon:
                            uncommonCount++;
                            break;
                        case Rarity.Rare:
                            rareCount++;
                            break;
                        case Rarity.Elite:
                            eliteCount++;
                            break;
                        case Rarity.Mythical:
                            mythicalCount++;
                            break;
                        case Rarity.Legendary:
                            legendaryCount++;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    uncatched++;
                }
                //
                // if (fishTest.Fish.FishBiteType == FishType.Carnivorous)
                // {
                //     carni++;
                // } 
                // else if (fishTest.Fish.FishBiteType == FishType.Herbivorous)
                // {
                //     herbi++;
                // }
                // else
                // {
                //     combo++;
                // }
            }

            sb.AppendLine($"Common: {commonCount}");
            sb.AppendLine($"Uncommon: {uncommonCount}");
            sb.AppendLine($"Rare: {rareCount}");
            sb.AppendLine($"Elite: {eliteCount}");
            sb.AppendLine($"Mythical: {mythicalCount}");
            sb.AppendLine($"Legendary: {legendaryCount}");
            sb.AppendLine($"Не поймано: {uncatched}");
            sb.AppendLine($"Комбо поймано: {combo}, плотоядных: {carni}, нехищных: {herbi}");

            await telegramBot.SendTextMessageAsync(AdminId, sb.ToString());
        }

        public bool Contains(Message message)
        {
            return message.From.Id == AdminId && Name.Any(p => p.Contains(message.Text));
        }
    }
}
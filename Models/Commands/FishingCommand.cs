using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Job;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Enum;

namespace TelegramAspBot.Models.Commands
{
    public class FishingCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/fishing" };
        
        private readonly Random _random = new Random();
        private readonly JobManager _jobManager;
        private readonly ILogger<FishingCommand> _logger;

        public FishingCommand(JobManager jobManager, ILogger<FishingCommand> logger)
        {
            _jobManager = jobManager;
            _logger = logger;
        }

        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, FishingCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            
            
            var user = message.From;
            var userName = message.Text;
            await using var dbContext = new ApplicationContext();

            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == user.Id);
            if (!character.Spot.IsActive)
            {
                await telegramBot.SendTextMessageAsync(character.TelegramId,
                    $"Данная локация закрыта, попробую половить рыбу в другой /spots");
                return;
            }
            
            switch (character.CharState)
            {
                case State.Idle:
                {
                    try
                    {
                        if (HasFishingRod(character))
                        {
                            var job = new FishingJob(telegramBot, _jobManager)
                            {
                                TelegramId = character.TelegramId
                            };
                            await telegramBot.SendTextMessageAsync(user.Id, $"Началась рыбалка, не зевай, рыба может клюнуть в любой момент!");

                            await StartFishing(character, dbContext, job);
                        }
                        else
                        {
                            await telegramBot.SendTextMessageAsync(user.Id,
                                $"Ты не взял с собой удочку, проверь свой рюкзак /fishingrod или загляни в магазин /shop");
                        }
                    }
                    catch (Exception error)
                    {
                        // ReSharper disable once ExplicitCallerInfoArgument
                        character.CharState = State.Idle;
                        await dbContext.SaveChangesAsync();
                        _logger.LogError($"Не ушло сообщение {user.Id}\n" 
                                + error.StackTrace);
                    }
                }
                    break;
                case State.Fishing:
                    if (_jobManager.GetFishJob(character) == null)
                    {
                        try
                        {
                            var job = new FishingJob(telegramBot, _jobManager)
                            {
                                TelegramId = character.TelegramId
                            };
                            await telegramBot.SendTextMessageAsync(user.Id, $"Началась рыбалка, не зевай, рыба может клюнуть в любой момент!");
                            await StartFishing(character, dbContext, job);
                        }
                        catch (Exception e)
                        {
                            // ReSharper disable once ExplicitCallerInfoArgument
                            character.CharState = State.Idle;
                            await dbContext.SaveChangesAsync();
                            _logger.LogError($"Не ушло сообщение {user.Id}\n" 
                                             + e.StackTrace);
                        }
                    }
                    else
                    {
                        await telegramBot.SendTextMessageAsync(character.TelegramId, $"Ты уже чем-то занят");
                    }
                    break;
                default:
                {
                    await telegramBot.SendTextMessageAsync(character.TelegramId, $"Ты уже чем-то занят");
                }
                    break;
            }
        }

        private async Task StartFishing(Character character, ApplicationContext dbContext, FishingJob job)
        {
            character.CharStat.FishingTry++;
            character.CharStat.SeasonFishingTry++;
            character.CharStat.PercentCatches();

            character.CharState = State.Fishing;
            character.FishingSessionGuid = Guid.NewGuid();
            await dbContext.SaveChangesAsync();

            _jobManager.SetNewJob(job);
        }

        private bool HasFishingRod(Character character)
        {
            return null != character.Backpack.BackpackItems.FirstOrDefault(p =>
                    p.IsEquipped && p.ItemType == ItemType.FishingRod);
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
            return character != null && Name.Any(p => p.Equals(message.Text));
        }
    }
}
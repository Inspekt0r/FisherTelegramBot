using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Models;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Enum;
using TelegramAspBot.Models.Interfaces;

namespace TelegramAspBot.Job
{
    public class JobManager
    {
        private List<IJob> Jobs { get; } = new List<IJob>();
        public readonly Guid SessionGuid = Guid.NewGuid();
        private readonly Random _random = new Random();
        private List<JobLure> JobLures { get; } = new List<JobLure>();
        private readonly ILogger<JobManager> _logger;
        private int _hourLast = 0;
        private int _hourNow;
        private bool IsCounting = false;

        private Season _currentSeason;
        private const int AdminId = 137968009;
        
        private readonly ITelegramBotClient _telegramBot;
        public JobManager(ILogger<JobManager> logger, IBotService botService)
        {
            _telegramBot = botService.GetBotClient();
            _logger = logger;
            _hourNow = DateTime.UtcNow.Hour;
            StartLureRead(_telegramBot);
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    await OnTimerElapsed();
                    await OnTimerElapsedLure();
                    await SeasonTimerElapsed();
                }
            });
        }

        private async Task SeasonTimerElapsed()
        {
            if (_currentSeason == null)
            {
                await using var dbContext = new ApplicationContext();
                var activeSeason = dbContext.SeasonStats.FirstOrDefault(p => !p.IsEnded);
                if (activeSeason == null)
                {
                    _currentSeason = new Season()
                    {
                        Month = DateTime.UtcNow.Month,
                        Number = 1
                    };
                    dbContext.Add(_currentSeason);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    _currentSeason = activeSeason;
                }
            }

            if (_currentSeason.Month < DateTime.UtcNow.Month || IsNewYear())
            {
                if (IsCounting)
                {
                    return;
                }

                IsCounting = true;
                
                await using var dbContext = new ApplicationContext();
                await _telegramBot.SendTextMessageAsync(AdminId, 
                    $"Новый месяц, пора считать достижения игроков в сезоне {_currentSeason.Number}");
                
                _currentSeason.IsEnded = true;
                var newSeason = new Season {Month = DateTime.UtcNow.Month, Number = _currentSeason.Number + 1};

                var allPlayers = dbContext.Characters.Where(p => p.IsSetupNickname && !p.Banned).ToList();
                var orderedBySeasonPoints = allPlayers.OrderByDescending(p => p.SeasonPoints).ToList();
                
                foreach (var character in allPlayers)
                {
                    var giftSystem = new SeasonGifts(character, _currentSeason, orderedBySeasonPoints);
                    try
                    {
                        await _telegramBot.SendTextMessageAsync(character.TelegramId, giftSystem.GetGiftForPlayer().ToString(), ParseMode.Html);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"{character.TelegramId} не получил сообщение\n{e.StackTrace}");
                    }
                }

                dbContext.Add(newSeason);
                var oldSeason = dbContext.SeasonStats.First(p => p.Id == _currentSeason.Id);
                oldSeason.IsEnded = true;

                _currentSeason = null;
                _currentSeason = newSeason;
                
                await dbContext.SaveChangesAsync();
                
                await _telegramBot.SendTextMessageAsync(AdminId, 
                    $"Я посчитал, новый сезон: {_currentSeason.Number}");
                
                IsCounting = false;
            }
        }

        private bool IsNewYear()
        {
            return _currentSeason.Month == 12 && DateTime.UtcNow.Month == 1;
        }
        private async Task UpdatePlayer()
        {
            await using var dbContext = new ApplicationContext();
            var charList = dbContext.Characters.Where(p => p.CharState == State.Fishing).ToList();
        }
        private void StartLureRead(ITelegramBotClient telegramBot)
        {
            using var db = new ApplicationContext();
            var checkLures = db.Lures.FirstOrDefault();
            if (checkLures == null)
            {
                return;
            }
            var lures = db.Lures.Where(p => p.LureTimeLeft >= DateTime.UtcNow).ToList();
            foreach (var lure in lures)
            {
                var jobLure = new JobLure(telegramBot)
                {
                    Character = lure.Character,
                    LureTimeLeft = lure.LureTimeLeft,
                    IsCompleted = lure.LureTimeLeft <= DateTime.UtcNow,
                    LureItem = lure.Item
                };
                
                JobLures.Add(jobLure);
            }
            _logger.LogInformation($"Люр модули в количестве: {JobLures.Count} были подняты");
        }
        
        //todo Сделать регулятор погоды
        private async Task UpdateWeather()
        {
            if (_hourNow > _hourLast)
            {
                await using var dbContext = new ApplicationContext();
                var spotList = dbContext.Spots.ToList();
                foreach (var spot in spotList)
                {
                    _logger.LogInformation($"Запрашиваю погоду для {spot.Name}");
                    try
                    {
                        
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Произошла ошибка при попытке взять погоду для {spot.Name}\n" + 
                                         e.StackTrace);
                    }
                    _logger.LogInformation($"Успешно обновил погоду для {spot.Name}");
                }

                _hourLast = _hourNow;
            }
            
            _hourNow = DateTime.UtcNow.Hour;
        }

        public bool SetHook(int telegramId)
        {
            var job = Jobs.FirstOrDefault(p => p.TelegramId == telegramId);
            
            if (job == null) return false;
            
            var fishJob = (FishingJob) job;

            fishJob.IsHooked = true;
            fishJob.IsPressed = true;
            return fishJob.IsHooked;
        }
        public void SetNewJob(FishingJob job)
        {
            //todo доработать проверочку, а че доработать? хз)0)
            if (job.TelegramId != 0 && job.TypeJob == TypeJob.None)
            {
                var timeToFish = _random.Next(90, 181) * 500; 
                job.TimeToDo = DateTime.UtcNow.AddMilliseconds(timeToFish);
                _logger.LogInformation($"Создал Hooking Job для {job.TelegramId}");
                
                Jobs.Add(job);
                _logger.LogInformation($"Добавил работу для {job.TelegramId} время выполнения {job.TimeToDo}");
            }
        }
        private async Task OnTimerElapsed()
        {
            var toRemove = new List<IJob>();
            
            for (int i = 0; i < Jobs.Count; i++)
            {
                //_logger.LogTrace($"Время выполнения работы для {Jobs[i].TelegramId} - {Jobs[i].TimeToDo}");
                if (Jobs[i].TimeToDo <= DateTime.UtcNow)
                {
                    var action = Jobs[i];
                    //_logger.LogInformation($"Выполняю запуск джобы рыбалки {action.TelegramId}");
                    await action.DoAction();

                    if (action.IsCompleted)
                    {
                        toRemove.Add(action);
                        continue;
                    }
                }
            }

            Jobs.RemoveAll(p => toRemove.Contains(p));
        }
        private async Task OnTimerElapsedLure()
        {
            var toRemove = new List<JobLure>();

            for (int i = 0; i < JobLures.Count; i++)
            {
                if (JobLures[i].LureTimeLeft <= DateTime.UtcNow)
                {
                    var action = JobLures[i];
                    _logger.LogInformation($"Выполняю запуск джобы приманки {action.Character.TelegramId}");
                    await action.DoAction();

                    if (action.IsCompleted)
                    {
                        toRemove.Add(action);
                        continue;
                    }
                }
            }

            JobLures.RemoveAll(p => toRemove.Contains(p));
        }
        public async Task AddLure(Character character, BackpackItem backPackLure, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();
            var globalMinutes = dbContext.GlobalSettings.First().LureTimeMinutes;

            if (backPackLure.ItemType != ItemType.Lure)
            {
                await telegramBot.SendTextMessageAsync(character.TelegramId,
                    $"Но это же не приманка(((");
                _logger.LogInformation($"Пытался активировать не приманку ID: {character.TelegramId}");
                return;
            }

            var newJobLure = new JobLure(telegramBot)
            {
                Character = character,
                LureTimeLeft = DateTime.UtcNow.AddMinutes(globalMinutes),
                IsCompleted = false,
                LureItem = backPackLure
            };

            try
            {
                if (!IsLureActive(character))
                {
                    JobLures.Add(newJobLure);
                    _logger.LogInformation($"Люр модуль активировался на 30 минут для {character.TelegramId}");
                    await telegramBot.SendTextMessageAsync(character.TelegramId,
                        $"Ты активировал приманку на {globalMinutes} мин.");
                    backPackLure.Count--;
                    if (backPackLure.Count == 0)
                    {
                        await telegramBot.SendTextMessageAsync(character.TelegramId,
                            $"Потрачена последняя {backPackLure.ItemName}");
                    }

                    character.Lure = new Lure()
                    {
                        LureTimeLeft = DateTime.UtcNow.AddMinutes(globalMinutes),
                        Item = backPackLure
                    };
                }
                else
                {
                    _logger.LogInformation($"Игрок {character.TelegramId} пытался запустить люр, но он уже активен");
                    await telegramBot.SendTextMessageAsync(character.TelegramId,
                        $"Приманка уже активна");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Люр модуль не добавился в работу для {character.TelegramId}");
                _logger.LogError(e.StackTrace);
                await telegramBot.SendTextMessageAsync(character.TelegramId,
                    $"Приманка поломанка, чет сломалось KEKW");
            }
        }

        public JobLure GetActiveLureJob(Character character)
        {
            return JobLures.FirstOrDefault(p => p.Character.TelegramId == character.TelegramId);
        }
        private bool IsLureActive(Character character)
        {
            foreach (var lure in JobLures)
            {
                if (lure.Character.TelegramId == character.TelegramId && !lure.IsCompleted)
                {
                    return true;
                }
            }

            return false;
        }

        public double GetLureRate(Character character)
        {
            foreach (var lure in JobLures)
            {
                if (lure.Character.TelegramId == character.TelegramId && !lure.IsCompleted)
                {
                    return lure.LureItem.CatchBonus;
                }
            }

            return 0.001;
        }

        public FishingJob GetFishJob(Character character)
        {
            var job = Jobs.FirstOrDefault(p => p.TelegramId == character.TelegramId);
            var fishJob = (FishingJob) job;

            return fishJob;
        }

        public void SetFishJobFishing(Character character)
        {
            var job = Jobs.FirstOrDefault(p => p.TelegramId == character.TelegramId);
            var fishJob = (FishingJob) job;

            if (fishJob != null)
            {
                fishJob.TypeJob = TypeJob.Fishing;
            }
        }
    }
    public static class JobManagerExtensions
    {
        public static IApplicationBuilder UseJobManager(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<JobManager>();
            return app;
        }
    }
}
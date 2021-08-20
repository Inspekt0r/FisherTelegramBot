using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.Fluent;
using Telegram.Bot;
using TelegramAspBot.Models;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Job
{
    public class LureManager
    {
        //todo реализовать класс менеджер приманок который будет добавлять действующую приманку у игрока
        /// <summary>
        /// Список всех джоб приманок
        /// </summary>
        private List<JobLure> _jobLures = new List<JobLure>();

        public LureManager()
        {
            //todo реализовать метод хранения всех активных люров в БД и загрузку их при старте
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    await OnTimerElapsed();
                }
            });
        }

        public async Task AddLure(Character character, BackpackItem backPackLure, ITelegramBotClient telegramBot)
        {
            //todo dbContext GlobalSettings.GetLureTime
            var globalMinutes = 30;

            if (backPackLure.ItemType != ItemType.Lure)
            {
                await telegramBot.SendTextMessageAsync(character.TelegramId,
                    $"Но это же не приманка(((");
                Log.Info($"Пытался активировать не приманку ID: {character.TelegramId}");
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
                    _jobLures.Add(newJobLure);
                    Log.Info($"Люр модуль активировался на {globalMinutes} минут для {character.TelegramId}");
                    await telegramBot.SendTextMessageAsync(character.TelegramId,
                        $"Ты активировал приманку на {globalMinutes} мин.");
                    backPackLure.Count--;
                    if (backPackLure.Count == 0)
                    {
                        await telegramBot.SendTextMessageAsync(character.TelegramId,
                            $"Потрачена последняя {backPackLure.ItemName}");
                    }
                }

                Log.Info($"Игрок {character.TelegramId} пытался запустить люр, но он уже активен");
                await telegramBot.SendTextMessageAsync(character.TelegramId,
                    $"Приманка уже активна");
            }
            catch (Exception e)
            {
                Log.Error($"Люр модуль не добавился в работу для {character.TelegramId}");
                Log.Error(e.StackTrace);
                await telegramBot.SendTextMessageAsync(character.TelegramId,
                    $"Приманка поломанка, чет сломалось KEKW");
            }
        }

        private bool IsLureActive(Character character)
        {
            foreach (var lure in _jobLures)
            {
                if (lure.Character.TelegramId == character.TelegramId && !lure.IsCompleted)
                {
                    return true;
                }
            }

            return false;
        }

        public void StartLureRead(ITelegramBotClient telegramBot)
        {
            using var db = new ApplicationContext();
            var lures = db.Lures.ToList();
            foreach (var lure in lures)
            {
                var jobLure = new JobLure(telegramBot)
                {
                    Character = lure.Character,
                    LureTimeLeft = lure.LureTimeLeft,
                    IsCompleted = lure.LureTimeLeft <= DateTime.UtcNow
                };
            }
        }

        private async Task OnTimerElapsed()
        {
            var toRemove = new List<JobLure>();

            for (int i = 0; i < _jobLures.Count; i++)
            {
                if (_jobLures[i].LureTimeLeft <= DateTime.UtcNow)
                {
                    var action = _jobLures[i];
                    await action.DoAction();

                    if (action.IsCompleted)
                    {
                        toRemove.Add(action);
                        continue;
                    }
                }
            }

            _jobLures.RemoveAll(p => toRemove.Contains(p));
        }
    }
}
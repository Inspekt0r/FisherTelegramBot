using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramAspBot.Job;

namespace TelegramAspBot.Models.Commands
{
    public class FishHookCallback : ICallback
    {
        public List<string> Name { get; } = new List<string>() {"hook"};
        private readonly JobManager _jobManager;
        private readonly ILogger<FishHookCallback> _logger;

        public FishHookCallback(JobManager jobManager, ILogger<FishHookCallback> logger)
        {
            _logger = logger;
            _jobManager = jobManager;
        }
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICallback, FishHookCallback>();
        }

        public async Task ExecuteCommand(CallbackQuery callback, ITelegramBotClient telegramBot)
        {
            var userId = callback.Message.Chat.Id;
            _logger.LogTrace($"Запускаю колбэк с данными {callback.Data} from {callback.From.Id}");
            await using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.First(p => p.TelegramId == userId);
            var splitData = callback.Data.Split(' ');
            var userGuidSession = GetSessionId(splitData[1]);
            //проверяем тайминги (если сообщение старше 2 минут то всё гг)
            if (callback.Message.Date >= DateTime.UtcNow.AddSeconds(121) || character.FishingSessionGuid.ToString() != userGuidSession)
            {
                _logger.LogTrace($"Время вышло для колбэка Дата колбэка: {callback.Message.Date} Время сервера: {DateTime.UtcNow} " +
                                 $"Дата до которой колбэк живёт: {DateTime.UtcNow.AddSeconds(121)}");
                await telegramBot.AnswerCallbackQueryAsync(callback.Id);
                await telegramBot.EditMessageTextAsync(userId, callback.Message.MessageId,
                    $"Рыбалка неожиданно закончилась, либо не началась", replyMarkup: null);
                return;
            }
            if (character.FishingSessionGuid.ToString() != userGuidSession)
            {
                _logger.LogWarning($"GUID рыбалки для {userId} не совпадает, " +
                                   $"текущий GUID: {character.FishingSessionGuid.ToString()} " +
                                   $"GUID из колбэка: {userGuidSession}");
                await telegramBot.AnswerCallbackQueryAsync(callback.Id);
                await telegramBot.EditMessageTextAsync(userId, callback.Message.MessageId,
                    $"Рыбалка неожиданно закончилась, либо не началась", replyMarkup: null);
                return;
            }
            _logger.LogTrace($"Получил {character}");
            var fishJob = _jobManager.GetFishJob(character);
            _logger.LogTrace($"FishJob null?: {fishJob==null}");
            if (fishJob == null)
            {
                await telegramBot.EditMessageTextAsync(userId, callback.Message.MessageId,
                    $"Рыбалка неожиданно закончилась, либо не началась", replyMarkup: null);
                return;
            }
            _logger.LogTrace($"Выставляю джобу по рыбалке {character.TelegramId}");
            _jobManager.SetFishJobFishing(character);
            _logger.LogTrace($"Выставил джобу по рыбалке {character.TelegramId}");
            var hookManager = new HookAndFishManager(character, _jobManager);
            _logger.LogTrace($"Создал HookAndFishManager {character.TelegramId}");
            await telegramBot.AnswerCallbackQueryAsync(callback.Id);
            _logger.LogTrace($"Отправил ответ на колбэк {character.TelegramId}");
            
            if (hookManager.IsHooked())
            {
                _logger.LogTrace($"Подсечка успешная {character.TelegramId}");
                if (_jobManager.SetHook(character.TelegramId))
                {
                    _logger.LogTrace($"Выставил в джобе успешную подсечку {character.TelegramId}");
                    character.CharStat.HookCount++;
                    character.CharStat.SeasonHookCount++;
                    await dbContext.SaveChangesAsync();
                    await telegramBot.EditMessageTextAsync(userId, callback.Message.MessageId, "ХОРОШАЯ ПОДСЕЧКА!",
                        replyMarkup: null);
                    await telegramBot.SendTextMessageAsync(userId, $"Подсечка удалась, осталось дотащить рыбину");
                }
                else
                {
                    await telegramBot.EditMessageTextAsync(userId, callback.Message.MessageId, $"А ты уже вроде рыбачишь, подожди немного", replyMarkup: null);
                }
            }
            else
            {
                await telegramBot.EditMessageTextAsync(userId, callback.Message.MessageId, $"Подсечка не удалась, но ты продолжаешь ловить рыбу, жди результат", replyMarkup: null);
            }
        }

        public bool Contains(CallbackQuery callback)
        {
            _logger.LogTrace($"Проверяю соответствие колбэка для {callback.From.Id}");
            var chatId = callback.Message.Chat.Id;
            _logger.LogTrace($"Чат {chatId}");
            var callBackCheck = false;
            foreach (var str in Name)
            {
                if (callback.Data.Contains(str))
                {
                    callBackCheck = true;
                    break;
                }
            }
            if (callBackCheck)
            {
                var splitData = callback.Data.Split(' ');
                var userIdFromHook = GetUserId(splitData[0]);
                var userGuidSession = GetSessionId(splitData[1]);
                //обращаемся к бд за персонажем
                using var dbContext = new ApplicationContext();
                var character = dbContext.Characters.First(p => p.TelegramId == chatId);
                _logger.LogTrace($"character: {character.TelegramId}");
                //смотрим тот ли пользователь отправил колбэк или нет
                if (!int.TryParse(userIdFromHook, out var hookId)) return false;
                if (hookId != chatId) return false;
                var fishingJob = _jobManager.GetFishJob(character);
                if (fishingJob!= null && fishingJob.IsPressed)
                {
                    _logger.LogTrace($"Джобы нет для колбэка");
                    return false;
                }
                return true;
            }
            _logger.LogTrace($"CallBackCheck не прошел {chatId}");
            return false;
        }

        private static string GetCommandFromCallback(string rawString)
        {
            return rawString.Split('_').First();
        }
        private static string GetUserId(string body)
        {
            return body.Split("hook_").Last();
        }

        private static string GetSessionId(string session)
        {
            return session.Split('_').Last();
        }
    }
}
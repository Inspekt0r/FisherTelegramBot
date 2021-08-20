using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Fluent;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Controllers;
using TelegramAspBot.Models;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Enum;
using ILogger = NLog.ILogger;

namespace TelegramAspBot.Job
{
    public enum TypeJob
    {
        None,
        Hooking,
        Fishing
    }

    public class FishingJob : IJob
    {
        private readonly ITelegramBotClient _telegramBot;
        private readonly JobManager _jobManager;
        private readonly Random _random = new Random();

        public FishingJob(ITelegramBotClient telegramBot, JobManager jobManager)
        {
            _telegramBot = telegramBot;
            _jobManager = jobManager;
        }

        /// <summary>
        /// Время когда джоба должна выполниться
        /// </summary>
        /// 
        public DateTime TimeToDo { get; set; }

        /// <summary>
        /// Флаг отвечающий подсёк рыбу персонаж или нет
        /// </summary>
        public bool IsHooked { get; set; }

        /// <summary>
        /// Показывает выполнена работа или нет
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Контекст БД в которой мы взяли 
        /// </summary>
        public ApplicationContext DbContext { get; set; }

        /// <summary>
        /// Персонаж который занят данной джобой (ID)
        /// </summary>
        public int TelegramId { get; set; }

        /// <summary>
        /// Прожата кнопка была ранее или нет
        /// </summary>
        public bool IsPressed { get; set; } = false;

        /// <summary>
        /// Тип джобы: None - пустая, Hooking - подсекает, Fishing - рыбачит
        /// </summary>
        public TypeJob TypeJob { get; set; } = TypeJob.None;

        public async Task DoAction()
        {
            if (TypeJob == TypeJob.None)
            {
                try
                {
                    await using var dbContext = new ApplicationContext();
                    var character = dbContext.Characters.First(p => p.TelegramId == TelegramId);

                    var timeToFish = _random.Next(60, 120) * 500;
                    await _telegramBot.SendTextMessageAsync(TelegramId,
                        $"ОГО ЧТО-ТО НАЧАЛО ДЁРГАТЬ ТВОЮ УДОЧКУ ПОДСЕКАЙ!",
                        replyMarkup: CallBackKeyboard.GetKeyboardFish(character, character.FishingSessionGuid));
                    TimeToDo = DateTime.UtcNow.AddMilliseconds(timeToFish);
                    TypeJob = TypeJob.Fishing;
                }
                catch (Exception e)
                {
                    if (e.StackTrace != null) Log.Error(e.StackTrace);

                    await using var dbContext = new ApplicationContext();
                    var character = dbContext.Characters.First(p => p.TelegramId == TelegramId);
                    character.CharState = State.Idle;
                    await dbContext.SaveChangesAsync();
                }

                return;
            }

            if (TypeJob == TypeJob.Fishing)
            {
                var jobFunction = new JobFunction(TelegramId, IsHooked, _jobManager);

                try
                {
                    await _telegramBot.SendTextMessageAsync(TelegramId, $"{jobFunction.GetFishingText()}", ParseMode.Html);
                    await CheckUnluckyCount();
                    IsCompleted = true;
                }
                catch (Exception e)
                {
                    if (e.StackTrace != null) Log.Error(e.StackTrace);
                    await using var dbContext = new ApplicationContext();
                    var character = dbContext.Characters.First(p => p.TelegramId == TelegramId);
                    character.CharState = State.Idle;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private async Task CheckUnluckyCount()
        {
            await using var db = new ApplicationContext();
            var character = db.Characters.FirstOrDefault(p => p.TelegramId == TelegramId);
            if (character == null)
            {
                return;
            }

            if (!character.HasStartedLure)
            {
                if (character.CharStat.UnluckyTry > 3)
                {
                    //todo создать в скрипте, а не каждый раз новую создавать
                    character.HasStartedLure = true;
                    var lure = db.Items.First(p => p.Name.Equals("Приманка новичка"));
                    var lureItem = lure.ConvertToBackpack();
                    lureItem.Count = 5;
                    character.Backpack.BackpackItems.Add(lureItem);

                    await _telegramBot.SendTextMessageAsync(character.TelegramId,
                        $"<i>Очередная сорвавшаяся рыба вызывает приступ боли в жопе</i>", ParseMode.Html);
                    await Task.Delay(2000);
                    await _telegramBot.SendTextMessageAsync(character.TelegramId,
                        $"<i>Вдруг к тебе издали начал кто-то идти, судя по всему какой-то матёрый рыбак</i>",
                        ParseMode.Html);
                    await _telegramBot.SendTextMessageAsync(character.TelegramId,
                        $"<b>Рыбак</b>: Смотрю у тебя совсем плохи дела, рыба не клюёт, держи приманку!",
                        ParseMode.Html);
                    await db.SaveChangesAsync();
                    await _telegramBot.SendTextMessageAsync(character.TelegramId, $"Ты получаешь {lure.Name} 5 штук!");
                    await Task.Delay(1000);
                    await _telegramBot.SendTextMessageAsync(character.TelegramId,
                        $"<b>Рыбак вернулся подсказать тебе как работает приманка</b>:\n" +
                        $"<i>-Короче, пока активна приманка, она даёт бонус к подсечке и к шансу выловить рыбу!\nАх, забыл, длительность у всех приманок одинаковая: где-то пол часа\n" +
                        $"Удачной ловли, рыбачок ха-ха-ха</i>\n" +
                        $"Открыть меню приманок: /fishlure", ParseMode.Html);
                }
            }
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Job;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Interfaces;

namespace TelegramAspBot.Models
{
    public class EventSystem
    {
        private bool _isActive;
        public string EventFish { get; private set; }
        public string EventItem { get; private set; }
        
        private readonly ITelegramBotClient _telegramBot;
        private readonly ILogger<EventSystem> _logger;
        private readonly Random _random = new Random();

        private const long ChannelId = -1001296152329;
        private const long MainChannelId = -1001328439037;

        //private const DayOfWeek DayStart = DayOfWeek.Saturday;
        private const int TimeToStart = 6;
        private const int TimeToEnd = 18;
        
        // private const int TimeToStartTest = 16;
        // private const int TimeToEndTest = 16;
        // private const DayOfWeek TestDay = DayOfWeek.Sunday;

        //private int _ticker = 0;

        private bool IsManualStart { get; set; }

        public EventSystem(IBotService botService, ILogger<EventSystem> logger)
        {
            _telegramBot = botService.GetBotClient();
            _logger = logger;
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    await EventManager();
                }
            });
        }

        public void StartManually()
        {
            IsManualStart = true;
        }

        private async Task EventManager()
        {
            await using var dbContext = new ApplicationContext();
            var activeEvent = dbContext.Events.FirstOrDefault(p => p.IsActive);
            
            if (activeEvent != null)
            {
                _isActive = activeEvent.IsActive;
                EventFish = activeEvent.FishName;
            }
            
            var timeNow = DateTime.UtcNow;
            if (timeNow.Hour == TimeToStart && !_isActive)
            {
                
                _logger.LogInformation($"Запускаю событие по ловле рыбы");
                
                var fishSystem = new FishPattern();
                var fish = fishSystem.EventFishList();
                EventFish = fish[_random.Next(0, fish.Count)].Name;
                
                if (IsItemsEvent(timeNow))
                {
                    //todo эвент на шмотки
                    var eventMsg = $"Поймай больше всех {EventFish} и получи предмет в награду.";
                    await _telegramBot.SendTextMessageAsync(ChannelId, eventMsg);
                    await SendEventPersonalMessages(dbContext, eventMsg);
                }
                else
                {
                    var eventMsg = $"Запускаю событие, поймай больше всех {EventFish} и получи награду!";
                    await _telegramBot.SendTextMessageAsync(ChannelId, eventMsg);
                    await SendEventPersonalMessages(dbContext, eventMsg);
                }
                
                
                _isActive = true;
                var eventEntity = new Event()
                {
                    FishName = EventFish,
                    IsActive = _isActive
                };

                dbContext.Add(eventEntity);
                await dbContext.SaveChangesAsync();
                
                _logger.LogInformation($"Событие успешно запущено, рыба события: {EventFish}");
            }

            if (timeNow.Hour == TimeToEnd && _isActive)
            {
                _logger.LogInformation($"Завершаю событие по ловле рыбы");
                var eventGift = new EventGifts(activeEvent.Id);
                var eventResult = eventGift.GetEventResult();
                await _telegramBot.SendTextMessageAsync(ChannelId, $"{eventResult}", ParseMode.Html);
                try
                {
                    await _telegramBot.SendTextMessageAsync(MainChannelId, $"{eventResult}", ParseMode.Html);
                }
                catch (Exception e)
                {
                    _logger.LogError($"{e.StackTrace}");
                    throw;
                }

                activeEvent.IsActive = false;
                await dbContext.SaveChangesAsync();
                
                _logger.LogInformation($"Событие успешно окончено");
                _isActive = false;
            }

            if (_isActive && 
                DateTime.UtcNow.Minute == 0 && 
                DateTime.UtcNow.Second == 0 &&
                DateTime.UtcNow.Hour != TimeToStart && 
                DateTime.UtcNow.Hour != TimeToEnd )
            {
                var eventGift = new EventGifts(activeEvent.Id);
                var tempResult = eventGift.GetTempResult();
                await _telegramBot.SendTextMessageAsync(ChannelId, $"{tempResult}", ParseMode.Html);
                try
                {
                    await _telegramBot.SendTextMessageAsync(MainChannelId, $"{tempResult}", ParseMode.Html);
                }
                catch (Exception e)
                {
                    _logger.LogError($"{e.StackTrace}");
                    throw;
                }
                
                _logger.LogInformation($"Разослал промежуточный результат");
            }

            if (IsManualStart && !_isActive)
            {
                IsManualStart = false;
                _logger.LogInformation($"Инициирован запуск события вручную");
            }
        }

        public static bool IsItemsEvent(DateTime timeNow)
        {
            return timeNow.DayOfWeek == DayOfWeek.Monday 
                   || timeNow.DayOfWeek == DayOfWeek.Friday;
        }

        private async Task SendEventPersonalMessages(ApplicationContext dbContext, string eventMessage)
        {
            var characters = dbContext.Characters.Where(p => p.SendPersonalAlert).ToList();
            _logger.LogInformation($"Начинаю рассылку личных сообщений о старте эвента игрокам ({characters.Count})");
            foreach (var character in characters)
            {
                try
                {
                    await _telegramBot.SendTextMessageAsync(character.TelegramId, $"{eventMessage}");
                }
                catch (Exception e)
                {
                    _logger.LogError($"{character} не получил сообщение\n{e.Message}");
                }
            }
            _logger.LogInformation($"Закончил рассылку игрокам сообщений в лс");
        }
    }
    
    public static class EventSystemExtensions
    {
        public static IApplicationBuilder UseEventSystem(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<EventSystem>();
            return app;
        }
    }
}
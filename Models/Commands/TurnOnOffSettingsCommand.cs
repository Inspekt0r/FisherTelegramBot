using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramAspBot.Models.Commands
{
    public class TurnOnOffSettingsCommand : ICommand
    {
        private readonly ILogger<TurnOnOffSettingsCommand> _logger;

        public TurnOnOffSettingsCommand(ILogger<TurnOnOffSettingsCommand> logger)
        {
            _logger = logger;
        }

        public List<string> Name { get; } = new List<string>() { "/turnoff_event", "/turnon_event" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, TurnOnOffSettingsCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();
            var userId = message.From.Id;
            var character = dbContext.Characters.First(p => p.TelegramId == userId);
            character.SendPersonalAlert = message.Text switch
            {
                "/turnoff_event" => false,
                "/turnon_event" => true,
                _ => character.SendPersonalAlert
            };
            _logger.LogInformation(
                $"Пришел запрос на смену настройки SendPersonalAlert для {character} на {character.SendPersonalAlert}");
            await dbContext.SaveChangesAsync();
            _logger.LogInformation($"Изменил состояние настройки SendPersonalAlert для {character} на {character.SendPersonalAlert}");
            await telegramBot.SendTextMessageAsync(userId,
                $"Поменял настройки на {TextGenerator.GetSettingEmoji(character.SendPersonalAlert)}");
        }

        public bool Contains(Message message)
        {
            _logger.LogInformation($"Пришло сообщение от {message.From}\nТекст сообщения: {message.Text}");
            if (message.Type != MessageType.Text || message.From.Id != message.Chat.Id)
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

            return Name.Any(command => character != null && message.Text.Equals(command));
        }
    }
}
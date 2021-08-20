using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.Commands
{
    public class SettingsCommand : ICommand
    {
        private readonly ILogger<SettingsCommand> _logger;
        public List<string> Name { get; } = new List<string>() { "/settings" };

        public SettingsCommand(ILogger<SettingsCommand> logger)
        {
            _logger = logger;
        }

        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, SettingsCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();
            var userId = message.From.Id;
            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == userId);
            var msgToSend = GenerateMessage(character);
            _logger.LogInformation($"Отправил сообщение для {character}\nТекст сообщения: {msgToSend}");
            await telegramBot.SendTextMessageAsync(userId, $"{msgToSend}",
                ParseMode.Html);
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

        private static string GenerateMessage(Character character)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<b>Настройки:</b>");
            if (character.SendPersonalAlert)
            {
                sb.AppendLine($"Уведомления о старте события {TextGenerator.GetSettingEmoji(character.SendPersonalAlert)} - отключить /turnoff_event");
            }
            else
            {
                sb.AppendLine($"Уведомления о старте события {TextGenerator.GetSettingEmoji(character.SendPersonalAlert)} - включить /turnon_event");
            }

            return sb.ToString();
        }
    }
}